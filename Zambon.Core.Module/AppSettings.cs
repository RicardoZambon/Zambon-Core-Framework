using System.Collections.Generic;

namespace Zambon.Core.Module
{
    public class AppSettings
    {

        #region Properties

        private IDictionary<string, string> ApplicationConfigs { get; set; }


        public string[] Languages { get { return Get("Languages")?.ToString()?.Split(";"); } }

        public string EnvironmentTitle { get { return Get("EnvironmentTitle")?.ToString(); } }

        public string FileStorePath { get { return Get("FileStorePath")?.ToString(); } }

        public string DefaultDomainName { get { return Get("DefaultDomainName")?.ToString(); } }

        public string ReportsURLServer { get { return Get("ReportsURLServer")?.ToString(); } }

        public string ReportsFolderBasePath { get { return Get("ReportsFolderBasePath")?.ToString(); } }

        #endregion

        #region Methods

        private object Get(string key)
        {
            return ApplicationConfigs.ContainsKey(key) ? ApplicationConfigs[key] : null;
        }

        public void Set(IDictionary<string, string> appSettings)
        {
            ApplicationConfigs = appSettings;
        }

        #endregion
    }
}