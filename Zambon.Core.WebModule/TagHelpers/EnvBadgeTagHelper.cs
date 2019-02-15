using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("span", Attributes = ForAttributeName)]
    public class EnvBadgeTagHelper : TagHelper
    {

        private const string ForAttributeName = "app-env-badge";

        #region Properties

        public override int Order => 0;

        protected ApplicationService App { get; }

        #endregion

        #region Constructors

        public EnvBadgeTagHelper(ApplicationService _app)
        {
            App = _app;
        }

        #endregion

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if(!string.IsNullOrWhiteSpace(App.GetAppSettings().EnvironmentTitle))
            {
                output.AddClass("border", HtmlEncoder.Default);
                output.AddClass("border-danger", HtmlEncoder.Default);
                output.AddClass("rounded", HtmlEncoder.Default);
                output.AddClass("bg-danger", HtmlEncoder.Default);
                output.AddClass("p-1", HtmlEncoder.Default);
                output.AddClass("mr-1", HtmlEncoder.Default);
                output.AddClass("text-white", HtmlEncoder.Default);
                output.Content.SetContent(App.GetAppSettings().EnvironmentTitle.ToUpper());
            }
            else
            {
                output.TagMode = TagMode.SelfClosing;
                output.TagName = "";
            }
        }

    }
}