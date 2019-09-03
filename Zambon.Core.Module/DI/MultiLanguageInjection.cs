using Microsoft.Extensions.DependencyInjection;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.DI
{
    /// <summary>
    /// Module injection methods to configure multi language services.
    /// </summary>
    public static class MultiLanguageInjection
    {
        /// <summary>
        /// Configure multi language services.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static void AddMultiLanguage<TLanguageProvider>(this IServiceCollection services) where TLanguageProvider : class, ILanguageProvider
        {
            services.AddSingleton<ILanguageProvider, TLanguageProvider>();
        }
    }
}