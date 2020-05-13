using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml.Serialization;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    public class ModelStore
    {
        private bool _modelsLoaded = false;

        private readonly AppSettings _appSettings;

        protected Dictionary<string, Application> AvailableModels { get; set; }



        public ModelStore(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            AvailableModels = new Dictionary<string, Application>();
        }


        protected void LoadModels(IModule mainModule)
        {
            var loadedModules = mainModule.LoadModules();

            foreach (var language in _appSettings.Languages)
            {
                var serializer = new XmlSerializer(typeof(Application));

                var culture = new CultureInfo(language);
                var model = SerializeModel(serializer, loadedModules, culture);

                if (model == null)
                {
                    throw new NullReferenceException($"Was not possible find any model for the language {language}.");
                }

                AvailableModels.Add(culture.Name, model);
            }

            _modelsLoaded = true;
        }

        protected Application SerializeModel(XmlSerializer serializer, IList<IModule> modules, CultureInfo language)
        {
            Application model = null;

            foreach (var module in modules)
            {
                try
                {
                    var assembly = module.GetType().Assembly;
                    if (Assembly.GetExecutingAssembly().GetCustomAttribute<NeutralResourcesLanguageAttribute>() is NeutralResourcesLanguageAttribute assemblyCulture
                        && new CultureInfo(assemblyCulture.CultureName).Name != language.Name)
                    {
                        assembly = assembly.GetSatelliteAssembly(CultureInfo.CreateSpecificCulture(language.Name));
                    }


                    using (var webModuleStream = assembly.GetManifestResourceStream($"{module.GetType().Assembly.GetName().Name}.{module.ApplicationModelName}.xml"))
                    {
                        if (webModuleStream != null && serializer.Deserialize(webModuleStream) is Application applicationModel)
                        {
                            if (model == null)
                            {
                                model = applicationModel;
                            }
                            else
                            {
                                model.Merge(applicationModel);
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    //Module is not using XML specific language, then, do nothing.
                }
            }
            return model;
        }


        public Application GetModel(IModule mainModule, string language)
        {
            if (!_modelsLoaded)
            {
                LoadModels(mainModule);
            }

            language = new CultureInfo(language).Name;
            if (!AvailableModels.ContainsKey(language))
            {
                throw new Exception($"No exists any model loaded with the language \"{language}\"");
            }

            return AvailableModels[language];
        }
    }
}