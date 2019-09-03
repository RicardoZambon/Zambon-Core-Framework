using System.Collections.Generic;

namespace Zambon.Core.Module
{
    /// <summary>
    /// Represents the AppSettings.json file
    /// </summary>
    public class CoreConfigs
    {
        #region Properties

        /// <summary>
        /// List of all configurations in "ApplicationConfigs" node with key and value.
        /// </summary>
        protected IDictionary<string, string> Configs { get; set; }


        /// <summary>
        /// List of languages in "Languages" already separated by ";". If no languages were informed will return null.
        /// </summary>
        public string[] Languages { get { return Get("Languages")?.ToString()?.Split(";"); } }

        /// <summary>
        /// If the application has at least one language informed in Languages section will return true.
        /// </summary>
        public bool IsMultilanguageApplication { get { return (Languages?.Length ?? 0) > 0; } }

        /// <summary>
        /// The name of the current environment, if production, then leave blank.
        /// </summary>
        public string EnvironmentTitle { get { return Get("EnvironmentTitle")?.ToString()?.ToUpper()?.Trim() ?? string.Empty; } }

        /// <summary>
        /// Will return true if the EnvironmentTitle section was informed and different than blank.
        /// </summary>
        public bool HasEnvironmentTitle { get { return !string.IsNullOrWhiteSpace(EnvironmentTitle); } }

        /// <summary>
        /// The physical path where application files should be stored.
        /// </summary>
        public string FileStorePath { get { return Get("FileStorePath")?.ToString(); } }

        /// <summary>
        /// The default domain name to validate the "LDAP" users.
        /// </summary>
        public string DefaultDomainName { get { return Get("DefaultDomainName")?.ToString(); } }

        /// <summary>
        /// The SSRS report server URL, will be used in menu options of the type "Report".
        /// </summary>
        public string ReportsURLServer { get { return Get("ReportsURLServer")?.ToString(); } }

        /// <summary>
        /// The SSRS folder base path of the reports, will be used in menu options of the type "Report".
        /// </summary>
        public string ReportsFolderBasePath { get { return Get("ReportsFolderBasePath")?.ToString(); } }

        #endregion

        #region Methods

        private object Get(string key)
        {
            return Configs.ContainsKey(key) ? Configs[key] : null;
        }

        /// <summary>
        /// Populate the configurations property with the AppSettings.json file. Should only get called when the application is starting.
        /// </summary>
        /// <param name="configs"></param>
        public void Set(IDictionary<string, string> configs)
        {
            Configs = configs;
        }

        #endregion
    }
}