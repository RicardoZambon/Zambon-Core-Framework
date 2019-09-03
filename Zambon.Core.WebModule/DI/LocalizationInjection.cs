using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.Localization;
using System.Globalization;
using System.Linq;
using Zambon.Core.WebModule.Services;

namespace Zambon.Core.Module.DI
{
    /// <summary>
    /// Module injection methods to configure multi language services.
    /// </summary>
    public static class LocalizationInjection
    {
        /// <summary>
        /// Configure multi language services.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static void AddLocalization(this IServiceCollection services)
        {
            services.AddMultiLanguage<LanguageProvider>();

            var serviceProvider = services.BuildServiceProvider();
            var coreConfigs = serviceProvider.GetService<CoreConfigsService>().Configs;
            if (coreConfigs.IsMultilanguageApplication)
            {
                var defaultCulture = coreConfigs.Languages[0];
                var supportedCultures = coreConfigs.Languages.Select(x => new CultureInfo(x)).ToArray();

                services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.DefaultRequestCulture = new RequestCulture(culture: defaultCulture, uiCulture: defaultCulture);
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;

                    options.RequestCultureProviders.Clear();
                    options.RequestCultureProviders.Add(new CoreRequestCultureProvider());
                });
            }
        }
    }
}