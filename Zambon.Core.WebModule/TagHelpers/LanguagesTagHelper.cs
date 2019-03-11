using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Web;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = "languages-container")]
    public class LanguagesTagHelper : TagHelper
    {
        [HtmlAttributeName("button-class")]
        public string ButtonCssClass { get; set; }

        [HtmlAttributeName("languages-use-button-style")]
        public string UseButtonStyle { get; set; }


        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly ApplicationService ApplicationService;
        private readonly ILanguageService LanguageService;
        protected IUrlHelperFactory UrlHelperFactory { get; }

        protected IUrlHelper UrlHelper { get { return UrlHelperFactory.GetUrlHelper(ViewContext); } }


        public LanguagesTagHelper(ApplicationService applicationService, ILanguageService languageService, IUrlHelperFactory urlHelperFactory)
        {
            ApplicationService = applicationService;
            LanguageService = languageService;
            UrlHelperFactory = urlHelperFactory;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((ApplicationService.AppConfigs.Languages?.Length ?? 0) == 0)
            {
                output.TagName = string.Empty;
                output.TagMode = TagMode.SelfClosing;
            }
            else
            {
                var currentLanguage = ApplicationService.GetLanguage(LanguageService.GetCurrentLanguage());
                if (currentLanguage == null)
                    currentLanguage = ApplicationService.GetLanguage(ApplicationService.AppConfigs.Languages[0]);

                var buttonClass = ButtonCssClass;
                if (!string.IsNullOrWhiteSpace(UseButtonStyle) && UseButtonStyle.Trim().ToLower() == "true")
                    buttonClass += $" btn btn-{(string.IsNullOrWhiteSpace(ApplicationService.AppConfigs.EnvironmentTitle) ? "primary" : "danger")}";

                var languageDropdown =
                    $"<a class=\"{buttonClass} language-selection dropdown-toggle\" href=\"#\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" title=\"{currentLanguage.DisplayName}\">" +
                        $"<span class=\"flag flag-{currentLanguage.FlagIcon} mr-1 align-middle\"></span>" +
                    "</a>" +
                    "<div class=\"dropdown-menu dropdown-menu-right\">";


                string returnUrl = HttpUtility.ParseQueryString(ViewContext.HttpContext.Request.QueryString.Value).Get("returnUrl");

                var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)ViewContext.ActionDescriptor).ControllerName ;

                for (var l = 0; l < ApplicationService.AppConfigs.Languages.Length; l++)
                {
                    var language = ApplicationService.GetLanguage(ApplicationService.AppConfigs.Languages[l]);
                    if (language != null && language.Code != currentLanguage.Code)
                    {
                        languageDropdown +=
                            $"<a class=\"dropdown-item pl-2 pr-2\" onclick=\"return ShowLoader(this);\" " +
                            $"href=\"{UrlHelper.Action("ChangeLanguage", "Home", new { newLanguage = language.Code, controllerName, returnUrl })}\" " +
                            $"title=\"{language.DisplayName}\">" +
                                $"<span class=\"flag flag-{language.FlagIcon} ml-2 mr-2 align-middle\"></span>" +
                            $"</a>";
                    }
                }
                languageDropdown += "</div>";

                output.Content.AppendHtml(languageDropdown);
            }

            base.Process(context, output);
        }

    }
}