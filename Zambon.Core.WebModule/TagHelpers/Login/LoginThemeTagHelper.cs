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
    [HtmlTargetElement("div", Attributes = ForAttributeName)]
    public class LoginTagHelper : TagHelper
    {

        private const string ForAttributeName = "app-set-login-theme";

        #region Properties

        public override int Order => 0;

        protected ApplicationService App { get; }

        #endregion

        #region Constructors

        public LoginTagHelper(ApplicationService _app)
        {
            App = _app;
        }

        #endregion

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.AddClass(App.GetLoginTheme(), HtmlEncoder.Default);
        }

    }
}