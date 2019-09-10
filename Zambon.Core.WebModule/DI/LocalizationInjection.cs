using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.Localization;
using System.Globalization;
using System.Linq;
using Zambon.Core.WebModule.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

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
                    options.RequestCultureProviders.Add(new CoreRequestCultureProvider() { Options = options, RouteDataStringKey = "lang", UIRouteDataStringKey = "lang" });
                });
            }
        }

        public static void ConfigureLocalization(this IApplicationBuilder app)
        {
            var coreConfigs = app.ApplicationServices.GetService<IOptions<CoreConfigs>>().Value;
            if (coreConfigs.IsMultilanguageApplication)
            {
                var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
                app.UseRequestLocalization(options);
            }
        }

        public static void ConfigureLocalizationFilter(this MvcOptions options, IServiceCollection services)
        {
            var coreConfigs = services.BuildServiceProvider().GetService<IOptions<CoreConfigs>>().Value;
            if (coreConfigs.IsMultilanguageApplication)
            {
                options.Filters.Add<LanguageFilter>();
            }
        }

        public static void ConfigureMvcLocalizationRoute(this IRouteBuilder route, IApplicationBuilder app)
        {
            var coreConfigs = app.ApplicationServices.GetService<IOptions<CoreConfigs>>().Value;
            if (coreConfigs.IsMultilanguageApplication)
            {
                route.MapRoute(
                    name: "Localized",
                    template: "{language}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index"}
                );
            }
        }
    }
}