using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Zambon.Core.Database.ChangeTracker.DI
{
    /// <summary>
    /// Dependency injection methods to configure the change tracker database cache.
    /// </summary>
    public static class CacheInjection
    {
        /// <summary>
        /// Extension method to configure the change tracker database cache.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="options">An System.Action`1 to configure the provided Microsoft.Extensions.Caching.SqlServer.SqlServerCacheOptions.</param>
        public static void AddChangeTrackerDbCache(this IServiceCollection services, Action<SqlServerCacheOptions> options)
        {
            services.AddDistributedSqlServerCache(options);
        }
    }
}