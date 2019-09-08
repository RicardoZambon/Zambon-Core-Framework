using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = "static-text*")]
    public class StaticTextTagHelper : TagHelper
    {
        #region Services

        protected readonly ILanguageProvider LanguageProvider;

        protected readonly ModelService ModelService;

        #endregion

        #region Properties

        [HtmlAttributeName("static-text-for")]
        public string For { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "static-text-")]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        #endregion

        #region Constructors

        public StaticTextTagHelper(ILanguageProvider languageProvider, ModelService modelService)
        {
            LanguageProvider = languageProvider;
            ModelService = modelService;
        }

        #endregion

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Attributes.Count() > 0)
            {
                foreach (var attribute in Attributes.Where(x => x.Key != "static-text-for"))
                {
                    output.Attributes.Add(attribute.Key.Replace("static-text-", ""), GetKeyValue(attribute.Value));
                }
            }
            if (!string.IsNullOrWhiteSpace(For) && GetKeyValue(For) is string staticTextValue && !string.IsNullOrEmpty(staticTextValue))
            {
                var content = output.GetChildContentAsync().Result;
                if (content.IsModified)
                {
                    content.AppendHtml(staticTextValue);
                    output.Content.SetHtmlContent(content.GetContent());
                }
                else
                {
                    output.Content.SetHtmlContent(staticTextValue);
                }
            }
            return base.ProcessAsync(context, output);
        }

        private string GetKeyValue(string key)
        {
            switch(key)
            {
                case "AppName":
                    return ModelService.AppName;
                case "AppMenuName":
                    return ModelService.AppMenuName;
                case "AppDescription":
                    return ModelService.AppDescription;
                case "Version":
                    return ModelService.Version;
                case "Copyright":
                    return ModelService.Copyright;
                default:
                    return ModelService.GetStaticText(key);
            }
        }

    }
}