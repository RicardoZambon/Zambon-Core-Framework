using Microsoft.Extensions.Options;
using Zambon.Core.Module.Extensions;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Zambon.Core.Module.Xml.Configuration;
using System.Text.RegularExpressions;
using Zambon.Core.Module.Xml.Languages;

namespace Zambon.Core.Module.Services
{
    /// <summary>
    /// Service used to across the entire application to read from the Application Model XML file.
    /// </summary>
    public class ModelService
    {
        #region Services

        private readonly CoreConfigs CoreConfigs;

        private readonly ILanguageProvider LanguageProvider;

        #endregion

        #region Variables

        private Dictionary<string, Application> LoadedModels;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the application. From ApplicationModel.xml, <Application /> node and [Name] attribute.
        /// </summary>
        public string AppName => CurrentModel.Name;

        /// <summary>
        /// The name of the application used in menu, if blank will use the Name property. From ApplicationModel.xml, <Application /> node and [MenuName] attribute.
        /// </summary>
        public string AppMenuName => string.IsNullOrEmpty(CurrentModel.MenuName) ? CurrentModel.Name : CurrentModel.MenuName;

        /// <summary>
        /// The description text for the application used in home page. From ApplicationModel.xml <Application /> node and [Description] attribute.
        /// </summary>
        public string AppDescription => CurrentModel.Description;


        private string _Version;
        /// <summary>
        /// The current version of the application, from the startup project Package > Package version.
        /// </summary>
        public string Version
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Version))
                {
                    var assembly = Assembly.GetEntryAssembly();
                    _Version = (assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).FirstOrDefault() as AssemblyInformationalVersionAttribute).InformationalVersion;
                    _Copyright = (assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).FirstOrDefault() as AssemblyCopyrightAttribute).Copyright;
                }
                return _Version;
            }
        }

        private string _Copyright;
        /// <summary>
        /// The copyright of the application, from the startup project Package > Copyright.
        /// </summary>
        public string Copyright
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Copyright))
                {
                    var assembly = Assembly.GetEntryAssembly();
                    _Version = (assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).FirstOrDefault() as AssemblyInformationalVersionAttribute).InformationalVersion;
                    _Copyright = (assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).FirstOrDefault() as AssemblyCopyrightAttribute).Copyright;
                }
                return _Copyright;

            }
        }


        private Application CurrentModel { get { return GetModel(LanguageProvider.GetCurrentLanguage()); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for the ApplicationService.
        /// </summary>
        /// <param name="coreConfigs">Instance of the AppSettings.json file.</param>
        /// <param name="languageProvider">Instance of the LanguageProvider service.</param>
        public ModelService(IOptions<CoreConfigs> coreConfigs, ILanguageProvider languageProvider)
        {
            CoreConfigs = coreConfigs.Value;
            LanguageProvider = languageProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a cloned instance of the Application Model.
        /// </summary>
        /// <param name="language">The current language of the model, if null will return the default language.</param>
        /// <returns>Return a cloned instance of the Application Model</returns>
        public Application GetModel(string language)
        {
            if ((LoadedModels?.Keys?.Count ?? 0) <= 0)
            {
                RefreshModelFiles();
            }
            if (LoadedModels == null)
            {
                throw new Exception("No application model was loaded, this was not expected!");
            }
            return string.IsNullOrWhiteSpace(language) || !LoadedModels.ContainsKey(language) ? LoadedModels.FirstOrDefault().Value : LoadedModels[language];
        }


        private void RefreshModelFiles()
        {
            if (LoadedModels == null)
            {
                LoadedModels = new Dictionary<string, Application>();
            }

            LoadedModels.Clear();
            var serializer = new XmlSerializer(typeof(Application));
            if (!CoreConfigs.IsMultilanguageApplication)
            {
                LoadedModels.Add("", LoadModelFile(serializer));
            }
            else
            {
                for (var l = 0; l < CoreConfigs.Languages.Length; l++)
                {
                    var language = l == 0 ? "" : CoreConfigs.Languages[l];
                    if (LoadModelFile(serializer, language) is Application model)
                    {
                        LoadedModels.Add(language, model);
                    }
                }
            }
        }

        private Application LoadModelFile(XmlSerializer serializer)
            => LoadModelFile(serializer, string.Empty);

        private Application LoadModelFile(XmlSerializer serializer, string language)
        {
            var assembly = Assembly.Load("Zambon.Core.WebModule");
            if (language != string.Empty)
            {
                assembly = assembly.GetSatelliteAssembly(CultureInfo.CreateSpecificCulture(language));
            }

            Application model = null;
            using (var webModuleStream = assembly.GetManifestResourceStream("Zambon.Core.WebModule.ApplicationModel.xml"))
            {
                if (webModuleStream != null && serializer.Deserialize(webModuleStream) is Application webApplicationModel)
                {
                    model = webApplicationModel;
                }
            }

            var fileName = string.Format("ApplicationModel{0}.xml", !string.IsNullOrWhiteSpace(language) ? "." + language : "");
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), fileName);
            using (var applicationStream = File.Exists(path) ? File.Open(path, FileMode.Open, FileAccess.Read) : Assembly.GetEntryAssembly().GetManifestResourceStream($"{Assembly.GetEntryAssembly().GetName().Name}.{fileName}"))
            {
                if (applicationStream != null && serializer.Deserialize(applicationStream) is Application applicationModel)
                {
                    if (model != null)
                    {
                        applicationModel.Merge(model);
                    }

                    if (LoadedModels.Count > 0)
                    {
                        applicationModel.Merge(LoadedModels.First().Value);
                    }

                    return applicationModel;
                }
            }

            return model;
        }

        #endregion

        #region Get model values methods

        /// <summary>
        /// Search all Static Texts using the Key property.
        /// </summary>
        /// <param name="key">The static text key to search.</param>
        /// <returns>Return the Static Text value string, string.Empty if not found.</returns>
        public string GetStaticText(string key)
        {
            if ((CurrentModel.StaticTexts?.Length ?? 0) > 0)
            {
                var returnStr = Array.Find(CurrentModel.StaticTexts, x => x.Key == key);
                if (!string.IsNullOrWhiteSpace(returnStr?.Value ?? string.Empty))
                {
                    return Regex.Replace(returnStr.Value, @"\[(.*?)\]", match => { return match.ToString().Replace("[", "<").Replace("]", ">"); });
                }
            }
            return string.Empty;
        }


        public Language[] GetLanguages()
            => CurrentModel.Languages;

        public bool HasLanguage(string language)
        {
            if ((CurrentModel.Languages?.Length ?? 0) > 0)
            {
                return Array.Exists(CurrentModel.Languages, l => l.Code.ToLower() == language?.Trim()?.ToLower());
            }
            return false;
        }

        public Language GetCurrentLanguage()
        {
            if ((CurrentModel.Languages?.Length ?? 0) > 0)
            {
                return Array.Find(CurrentModel.Languages, l => l.Code.ToLower() == LanguageProvider.GetCurrentLanguage().ToLower());
            }
            return null;
        }

        public CultureInfo GetCurrentCulture()
            => new CultureInfo(LanguageProvider.GetCurrentLanguage());


        /// <summary>
        /// Return the <ApplicationModel /> node values.
        /// </summary>
        /// <returns>Return the <ApplicationModel /> node values.</returns>
        public ModuleConfiguration GetConfiguration() => CurrentModel.ModuleConfiguration;

        #endregion
    }
}