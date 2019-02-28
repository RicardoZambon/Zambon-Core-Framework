using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = "static-text*")]
    public class StaticTextsTagHelper : TagHelper
    {

        #region Properties

        [HtmlAttributeName("static-text-for")]
        public string For { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "static-text-")]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        protected ApplicationService ApplicationService { get; }

        #endregion

        #region Constructors

        public StaticTextsTagHelper(ApplicationService ApplicationService)
        {
            this.ApplicationService = ApplicationService;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Attributes.Count() > 0)
                foreach (var attribute in Attributes.Where(x => !x.Key.StartsWith("replace-")))
                {
                    var staticValue = GetKeyValue(attribute.Value);

                    var replaceValues = Attributes.Where(x => x.Key.StartsWith($"replace-{attribute.Key}-"));
                    if (replaceValues.Count() > 0)
                        foreach (var replace in replaceValues)
                        {
                            var key = replace.Key.Replace($"replace-{attribute.Key}-", "");
                            staticValue = staticValue.Replace($"{{{key}}}", replace.Value);
                        }

                    output.Attributes.Add(attribute.Key, staticValue);
                }

            if (!string.IsNullOrWhiteSpace(For))
            {
                var staticValue = GetKeyValue(For);

                var replaceValues = Attributes.Where(x => x.Key.StartsWith("replace-") && !x.Key.Replace("replace-", "").Contains("-"));
                if (replaceValues.Count() > 0)
                    foreach (var replace in replaceValues)
                    {
                        var key = replace.Key.Replace($"replace-", "");
                        staticValue = staticValue.Replace($"{{{key}}}", replace.Value);
                    }

                output.Content.SetHtmlContent(staticValue);
            }
        }

        private string GetKeyValue(string key)
        {
            switch(key)
            {
                case "AppName":
                    return ApplicationService.AppName;
                case "AppMenuName":
                    return ApplicationService.AppMenuName;
                case "AppFullName":
                    return ApplicationService.AppFullName;
                case "Version":
                    return ApplicationService.Version;
                case "Copyright":
                    return ApplicationService.Copyright;
                default:
                    return ApplicationService.GetStaticText(key);
            }
        }

    }
}