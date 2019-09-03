using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = AppendVersionAttribute)]
    public class AppendVersionTagHelper : TagHelper
    {
        #region Services

        private readonly IUrlHelperFactory UrlHelperFactory;

        #endregion

        #region Properties

        private const string AppendVersionAttribute = "core-append-version";

        [HtmlAttributeName(AppendVersionAttribute)]
        public string _appendVersion { get; set; }

        public bool AppendVersion { get { return bool.TryParse(_appendVersion, out var result) ? result : false; } }


        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion

        #region Constructors

        public AppendVersionTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            UrlHelperFactory = urlHelperFactory;
        }

        #endregion

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = string.Empty;
            switch(output.TagName)
            {
                case "link":
                    attribute = "href";
                    break;
                case "script":
                    attribute = "src";
                    break;
            }

            if (attribute != string.Empty)
            {
                var urlHelper = UrlHelperFactory.GetUrlHelper(ViewContext);
                var linkValue = context.AllAttributes[attribute].Value.ToString();
                if (urlHelper.IsLocalUrl(linkValue))
                {
                    if (linkValue.Contains("?"))
                    {
                        linkValue += "&";
                    }
                    else
                    {
                        linkValue += "?";
                    }

                    var version = (typeof(AppendVersionTagHelper).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).FirstOrDefault() as AssemblyInformationalVersionAttribute).InformationalVersion;
                    output.Attributes.Remove(output.Attributes[attribute]);
                    output.Attributes.Add(attribute, urlHelper.Content(linkValue) + $"version={version}");
                }
            }
            return base.ProcessAsync(context, output);
        }
    }
}