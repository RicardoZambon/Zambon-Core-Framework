using Microsoft.Extensions.DependencyInjection;
using Zambon.Core.Database.ChangeTracker.Services;

namespace Zambon.Core.Database.ChangeTracker.DI
{
    /// <summary>
    /// Dependency injection methods to configure the change tracker general services.
    /// </summary>
    public static class ServicesInjection
    {
        /// <summary>
        /// Extension method to configure the change tracker general services.
        /// </summary>
        /// <typeparam name="TInstanceKeyService">The object type descendant from IInstanceKeyService to use.</typeparam>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static void AddChangeTrackerServices<TInstanceKeyService>(this IServiceCollection services) where TInstanceKeyService : class, IInstanceKeyService
        {
            services.AddSingleton<IInstanceKeyService, TInstanceKeyService>();
            services.AddSingleton<CoreChangeTracker>();
        }
    }
}