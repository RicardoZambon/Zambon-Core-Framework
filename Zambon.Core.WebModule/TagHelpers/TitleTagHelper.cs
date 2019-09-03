using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("title")]
    public class TitleTagHelper : TagHelper
    {
        #region Services

        private readonly ApplicationService AppService;

        #endregion

        #region Constructors

        public TitleTagHelper(ApplicationService appService)
        {
            AppService = appService;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.Append(AppService.GetApplicationTitle());
            base.Process(context, output);
        }
    }
}