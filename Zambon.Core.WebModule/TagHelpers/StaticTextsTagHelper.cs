using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = ForAttributeName)]
    public class StaticTextsTagHelper : TagHelper
    {

        private const string ForAttributeName = "static-text*";

        #region Properties

        public override int Order => 0;

        [HtmlAttributeName("static-text-for")]
        public string For { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "static-text-")]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        protected ApplicationService App { get; }

        #endregion

        #region Constructors

        public StaticTextsTagHelper(ApplicationService _app)
        {
            App = _app;
        }

        #endregion

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

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

                var replaceValues = Attributes.Where(x => x.Key.StartsWith("replace-") && !x.Key.Replace("replace-","").Contains("-"));
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
                    return App.GetAppName();
                case "AppMenuName":
                    return App.GetAppMenuName();
                case "AppVersion":
                    return App.GetAppVersion();
                case "AppCopyright":
                    return App.GetAppCopyright();
                default:
                    return App.GetStaticText(key);
            }
        }

    }
}