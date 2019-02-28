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
    [HtmlTargetElement("div", Attributes = "app-set-login-theme")]
    public class LoginTagHelper : TagHelper
    {

        #region Properties

        private readonly ApplicationService ApplicationService;

        #endregion

        #region Constructors

        public LoginTagHelper(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddClass(ApplicationService.LoginTheme, HtmlEncoder.Default);
        }

    }
}