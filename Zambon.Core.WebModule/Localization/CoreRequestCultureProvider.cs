using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.Localization
{
    public class CoreRequestCultureProvider : RouteDataRequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var serviceProvider = httpContext.Features.Get<IServiceProvidersFeature>();
            var localization = (serviceProvider.RequestServices.GetService(typeof(IOptions<RequestLocalizationOptions>)) as IOptions<RequestLocalizationOptions>).Value;
            var languageProvider = serviceProvider.RequestServices.GetService(typeof(ILanguageProvider)) as ILanguageProvider;

            if (localization == null || languageProvider == null)
            {
                return Task.FromResult((ProviderCultureResult)null);
            }

            var currentCulture = languageProvider.GetCurrentLanguage();
            if (currentCulture == string.Empty)
            {
                return Task.FromResult(new ProviderCultureResult(localization.DefaultRequestCulture.Culture.Name, localization.DefaultRequestCulture.UICulture.Name));
            }

            return Task.FromResult(new ProviderCultureResult(currentCulture));
        }
    }
}