using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Xml.Serialization;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Exceptions;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.Module.Services
{
    public abstract class ModelProviderBase<TApplication> : IModelProvider where TApplication : class, IApplication, new()
    {
        #region Variables

        private bool _modelsLoaded = false;

        #endregion

        #region Services

        private readonly AppSettings _appSettings;
        private readonly IModule _mainModule;

        #endregion

        #region Properties

        private CultureInfo defaultCulture;
        /// <summary>
        /// Default culture used by executing assembly.
        /// </summary>
        public CultureInfo DefaultCulture
        {
            get
            {
                if (defaultCulture == null)
                {
                    defaultCulture = new CultureInfo(Assembly.GetExecutingAssembly().GetCustomAttribute<NeutralResourcesLanguageAttribute>().CultureName);
                }
                return defaultCulture;
            }
        }

        protected Dictionary<string, TApplication> AvailableModels { get; set; }

        #endregion

        #region Constructors

        public ModelProviderBase(IOptions<AppSettings> appSettings, IModule mainModule)
        {
            _appSettings = appSettings.Value;

            AvailableModels = new Dictionary<string, TApplication>();
            this._mainModule = mainModule;
        }

        #endregion

        #region Methods

        protected void LoadModels()
        {
            if (!_modelsLoaded)
            {
                var loadedModules = _mainModule.LoadModules();

                var languages = new List<CultureInfo>() { DefaultCulture };
                languages.AddRange(_appSettings.Languages.Select(x => new CultureInfo(x)).Where(x => x.Name != DefaultCulture.Name));

                foreach (var language in languages)
                {
                    if (!AvailableModels.ContainsKey(language.Name))
                    {
                        var model = SerializeModel(_mainModule.ApplicationModelType, loadedModules, language);

                        if (model == null)
                        {
                            throw new NullReferenceException($"Was not possible find any model for the language {language.Name}.");
                        }
                        else if (language.Name != DefaultCulture.Name)
                        {
                            model.Merge(AvailableModels[DefaultCulture.Name]);
                        }

                        AvailableModels.Add(language.Name, model);
                    }
                }
                _modelsLoaded = true;
            }
        }

        protected TApplication SerializeModel(Type mainModelType, IList<IModule> modules, CultureInfo language)
        {
            TApplication model = null;
            foreach (var module in modules)
            {
                try
                {
                    var assembly = module.GetType().Assembly;
                    if (DefaultCulture.Name != language.Name)
                    {
                        assembly = assembly.GetSatelliteAssembly(CultureInfo.CreateSpecificCulture(language.Name));
                    }

                    using (var webModuleStream = assembly.GetManifestResourceStream($"{module.GetType().Assembly.GetName().Name}.{module.ApplicationModelName}.xml"))
                    {
                        var serializer = new XmlSerializer(module.ApplicationModelType);

                        if (webModuleStream != null)
                        {
                            var applicationModel = serializer.Deserialize(webModuleStream);

                            if (module.ApplicationModelType != mainModelType && model == null)
                            {
                                model = new TApplication();
                            }

                            if (model == null)
                            {
                                model = (TApplication)applicationModel;
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

        /// <summary>
        /// Search for the specific model language and return it.
        /// </summary>
        /// <param name="language">The language of the model.</param>
        /// <returns>Return the application model.</returns>
        public TApplication GetModel(string language)
        {
            if (!_modelsLoaded)
            {
                LoadModels();
            }

            language = new CultureInfo(language).Name;
            if (!AvailableModels.ContainsKey(language))
            {
                throw new ModelNotFoundException(language);
            }

            return AvailableModels[language];
        }

        object IModelProvider.GetModel(string language)
            => GetModel(language);

        #endregion
    }
}