using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zambon.Core.Module
{
    public class AppSettings
    {

        #region Properties

        public Dictionary<string, string> ApplicationConfigs { get; set; }


        public string EnvironmentTitle { get { return ApplicationConfigs.ContainsKey("EnvironmentTitle") ? ApplicationConfigs["EnvironmentTitle"].ToString() : string.Empty; } }

        public string FileStorePath { get { return ApplicationConfigs.ContainsKey("FileStorePath") ? ApplicationConfigs["FileStorePath"].ToString() : string.Empty; } }

        public string DefaultDomainName { get { return ApplicationConfigs.ContainsKey("DefaultDomainName") ? ApplicationConfigs["DefaultDomainName"].ToString() : string.Empty; } }

        public string ReportsURLServer { get { return ApplicationConfigs.ContainsKey("ReportsURLServer") ? ApplicationConfigs["ReportsURLServer"].ToString() : string.Empty; } }

        public string ReportsFolderBasePath { get { return ApplicationConfigs.ContainsKey("ReportsFolderBasePath") ? ApplicationConfigs["ReportsFolderBasePath"].ToString() : string.Empty; } }

        #endregion

    }
}
