using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Zambon.Core.Module.DI
{
    /// <summary>
    /// Dependency injection methods to configure the application configurations.
    /// </summary>
    public static class ConfigsInjection
    {
        /// <summary>
        /// Extension method to configure the application configurations.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="Configuration">The configuration instance.</param>
        public static void AddCoreConfigs<TCoreConfigs>(this IServiceCollection services, IConfigurationRoot Configuration) where TCoreConfigs : CoreConfigs
        {
            services.Configure<TCoreConfigs>(a =>
            {
                a.Set(Configuration.GetSection(nameof(CoreConfigs)).AsEnumerable().Where(x => x.Key != nameof(CoreConfigs)).ToDictionary(x => x.Key.Replace($"{nameof(CoreConfigs)}:", ""), y => y.Value));
            });
        }
    }
}