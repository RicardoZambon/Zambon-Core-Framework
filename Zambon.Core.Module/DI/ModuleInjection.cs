using Microsoft.Extensions.DependencyInjection;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.DI
{
    /// <summary>
    /// Module injection methods to configure base model services.
    /// </summary>
    public static class ModuleInjection
    {
        /// <summary>
        /// Configure base model services.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static void AddApplicationModel(this IServiceCollection services)
        {
            services.AddSingleton<CoreConfigsService>();
            services.AddSingleton<ModelService>();
            services.AddScoped<ApplicationService>();
            //services.AddScoped<ExpressionsService>();
        }
    }
}