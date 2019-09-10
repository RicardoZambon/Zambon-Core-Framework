using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.Localization
{
    public class LanguageFilter : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.RequestServices.GetService(typeof(ILanguageProvider)) is ILanguageProvider languageProvider
                && context.HttpContext.RequestServices.GetService(typeof(ModelService)) is ModelService modelService
                && context.RouteData.Values.Keys.Contains("language"))
            {
                var language = context.RouteData.Values["language"].ToString()?.Trim()?.ToLower();
                if (!string.IsNullOrEmpty(language) && modelService.HasLanguage(language))
                {
                    languageProvider.ChangeLanguage(language);
                }
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
        }
    }
}
