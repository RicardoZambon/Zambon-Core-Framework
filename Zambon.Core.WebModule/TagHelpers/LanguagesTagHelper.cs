using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Encodings.Web;
using Zambon.Core.Database.Domain.ExtensionMethods;
using Zambon.Core.Module;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = "core-languages")]
    public class LanguagesTagHelper : TagHelper
    {
        #region Services

        protected readonly LinkGenerator LinkGenerator;

        protected readonly CoreConfigs CoreConfigs;

        protected readonly ModelService ModelService;

        protected readonly ILanguageProvider LanguageService;

        #endregion

        #region Properties

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion

        #region Constructors

        public LanguagesTagHelper(LinkGenerator linkGenerator, IOptions<CoreConfigs> coreConfigs, ModelService modelService, ILanguageProvider languageProvider)
        {
            LinkGenerator = linkGenerator;
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

                var routeData = ViewContext.HttpContext.GetRouteData().Values;

                for (var l = 0; l < languages.Length; l++)
                {
                    var language = languages[l];
                    if (language != null && language.Code.ToLower() != currentLanguage.Code.ToLower())
                    {
                        stringBuilder
                            .Append("<li class=\"dropdown-item\">")
                            .Append($"<a href=\"{LinkGenerator.GetPathByRouteValues(ViewContext.HttpContext, "Localized", routeData.AddOrUpdate("language", language.Code))}\" class=\"btn btn-flag fp fp-lg fp-rounded {language.FlagIcon}\" title=\"{language.DisplayName}\"></a>")
                            .Append("</li>");
                    }
                }
                stringBuilder.Append("</ul>");
                output.Content.AppendHtml(stringBuilder.ToString());
            }
            base.Process(context, output);
        }
    }
}