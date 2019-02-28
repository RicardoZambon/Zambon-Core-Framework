using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = "app-env-badge")]
    public class EnvBadgeTagHelper : TagHelper
    {

        private readonly ApplicationService ApplicationService;


        public EnvBadgeTagHelper(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!ApplicationService.AppConfigs.HasEnvironmentTitle)
            {
                output.TagMode = TagMode.SelfClosing;
                output.TagName = "";
            }
            else
                output.Content.AppendHtml($"<div class=\"border border-danger rounded bg-danger p-1 text-white h4 mb-0\">{ApplicationService.AppConfigs.EnvironmentTitle}</div>");
        }

    }
}