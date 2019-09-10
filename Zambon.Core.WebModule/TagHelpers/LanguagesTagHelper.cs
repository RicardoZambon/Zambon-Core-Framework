using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Encodings.Web;
using Zambon.Core.Module;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = "core-languages")]
    public class LanguagesTagHelper : TagHelper
    {
        #region Services

        protected readonly IUrlHelperFactory UrlHelperFactory;

        protected readonly CoreConfigs CoreConfigs;

        protected readonly ModelService ModelService;

        protected readonly ILanguageProvider LanguageService;

        #endregion

        #region Properties

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion

        #region Constructors

        public LanguagesTagHelper(IUrlHelperFactory urlHelperFactory, IOptions<CoreConfigs> coreConfigs, ModelService modelService, ILanguageProvider languageProvider)
        {
            UrlHelperFactory = urlHelperFactory;
            CoreConfigs = coreConfigs.Value;
            ModelService = modelService;
            LanguageService = languageProvider;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var languages = ModelService.GetLanguages();
            if (!CoreConfigs.IsMultilanguageApplication && (languages?.Length ?? 0) > 0)
            {
                output.TagName = string.Empty;
                output.TagMode = TagMode.SelfClosing;
            }
            else
            {
                var currentLanguage = ModelService.GetCurrentLanguage() ?? languages[0];

                var stringBuilder = new StringBuilder();
                stringBuilder
                    .Append($"<a href=\"#\" class=\"btn btn-flag fp fp-lg fp-rounded {currentLanguage.FlagIcon}\" data-toggle=\"dropdown\" title=\"{ModelService.GetStaticText("LanguageButtonTitle")}\"></a>")
                    .Append($"<ul class=\"dropdown-menu dropdown-menu-right\">");

                output.AddClass("nav-item", HtmlEncoder.Default);
                output.AddClass("dropdown", HtmlEncoder.Default);
                output.AddClass("x-2", HtmlEncoder.Default);

                var urlHelper = UrlHelperFactory.GetUrlHelper(ViewContext);

                //string returnUrl = HttpUtility.ParseQueryString(ViewContext.HttpContext.Request.QueryString.Value).Get("returnUrl");
                //var controllerName = ((ControllerActionDescriptor)ViewContext.ActionDescriptor).ControllerName;

                for (var l = 0; l < languages.Length; l++)
                {
                    var language = languages[l];
                    if (language != null && language.Code.ToLower() != currentLanguage.Code.ToLower())
                    {
                        stringBuilder
                            .Append("<li class=\"dropdown-item\">")
                            .Append($"<a href=\"#\" class=\"btn btn-flag fp fp-lg fp-rounded {language.FlagIcon}\" data-toggle=\"dropdown\" title=\"{language.DisplayName}\"></a>")
                            .Append("</li>");

                        //stringBuilder
                        //    .Append($"<a class=\"dropdown-item pl-2 pr-2\" onclick=\"return ShowLoader(this);\" ")
                        //    .Append($"href=\"{urlHelper.Action("ChangeLanguage", "Home", new { newLanguage = language.Code, controllerName, returnUrl })}\" ")
                        //    .Append($"title=\"{language.DisplayName}\">")
                        //        .Append($"<span class=\"flag flag-{language.FlagIcon} ml-2 mr-2 align-middle\"></span>")
                        //    .Append($"</a>");
                    }
                }
                stringBuilder.Append("</ul>");
                output.Content.AppendHtml(stringBuilder.ToString());
            }
            base.Process(context, output);
        }
    }
}