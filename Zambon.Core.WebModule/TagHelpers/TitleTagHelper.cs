using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("title")]
    public class TitleTagHelper : TagHelper
    {

        private readonly ApplicationService _app;

        #region Constructors

        public TitleTagHelper(ApplicationService app)
        {
            _app = app;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.Content.Append(string.Format("{0}{1} v{2}", _app.GetAppName(), (!string.IsNullOrWhiteSpace(_app.GetAppSettings().EnvironmentTitle) ? " " + _app.GetAppSettings().EnvironmentTitle.Trim() : string.Empty), _app.GetAppVersion()));
        }


    }
}