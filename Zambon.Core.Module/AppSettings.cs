using System.Collections.Generic;

namespace Zambon.Core.Module
{
    /// <summary>
    /// Represents the AppSettings.json file
    /// </summary>
    public class ApplicationConfigs
    {
        /// <summary>
        /// List of all configurations in "ApplicationConfigs" node with the key and value.
        /// </summary>
        protected IDictionary<string, string> Configs { get; set; }


        /// <summary>
        /// List of languages in "Languages" already separated by ";". If no languages were informed will return null.
        /// </summary>
        public string[] Languages => Get(nameof(Languages))?.ToString()?.Split(";");

        /// <summary>
        /// The name of the current environment.
        /// </summary>
        public string EnvironmentTitle => Get(nameof(EnvironmentTitle))?.ToString()?.ToUpper()?.Trim() ?? string.Empty;

        /// <summary>
        /// Will return true if the EnvironmentTitle was informed.
        /// </summary>
        public bool HasEnvironmentTitle => !string.IsNullOrWhiteSpace(EnvironmentTitle);

        /// <summary>
        /// The path where the files from the application should be stored.
        /// </summary>
        public string FileStorePath => Get(nameof(FileStorePath))?.ToString();

        /// <summary>
        /// The default domain name where the users with "LDAP" should be validated.
        /// </summary>
        public string DefaultDomainName => Get(nameof(DefaultDomainName))?.ToString();

        /// <summary>
        /// The URL of the report server, will be used in menu options of the type "Report".
        /// </summary>
        public string ReportsURLServer => Get(nameof(ReportsURLServer))?.ToString();

        /// <summary>
        /// The folder base path of the reports in report server, will be used in menu options of the type "Report".
        /// </summary>
        public string ReportsFolderBasePath => Get(nameof(ReportsFolderBasePath))?.ToString();


        #region Methods

        private object Get(string key)
            => Configs.ContainsKey(key) ? Configs[key] : null;

        /// <summary>
        /// Populate the configurations dictionary with the AppSettings.json file. Should only called when the application is starting.
        /// </summary>
        /// <param name="configs"></param>
        public void Set(IDictionary<string, string> configs)
        {
            Configs = configs;
        }

        #endregion
    }
}