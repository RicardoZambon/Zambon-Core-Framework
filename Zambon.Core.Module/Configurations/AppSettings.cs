using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Configurations
{
    public class AppSettings : IAppSettings
    {
        public string DefaultLanguage { get; set; }

        public string[] Languages { get; set; }

        public Environment Environment { get; set; }

        //IDictionary<string, string> Configs { get; set; }

        //public string Get(string key)
        //    => (Configs?.ContainsKey(key) ?? false) ? Configs[key] : null;
    }
}