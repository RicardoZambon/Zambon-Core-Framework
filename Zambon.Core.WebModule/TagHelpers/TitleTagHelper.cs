using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("title")]
    public class TitleTagHelper : TagHelper
    {

        private readonly ApplicationService ApplicationService;


        public TitleTagHelper(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.Append($"{(ApplicationService.AppConfigs.HasEnvironmentTitle ? "[" + ApplicationService.AppConfigs.EnvironmentTitle + "] " : "")}{ApplicationService.AppName} v{ApplicationService.Version}");
        }

    }
}