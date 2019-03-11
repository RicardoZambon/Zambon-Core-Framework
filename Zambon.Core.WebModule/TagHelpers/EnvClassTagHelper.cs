using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement(Attributes = "env-*")]
    public class EnvClassTagHelper : TagHelper
    {
        [HtmlAttributeName(DictionaryAttributePrefix = "env-class-")]
        public IDictionary<string, string> Environments { get; set; } = new Dictionary<string, string>();

        private readonly ApplicationService ApplicationService;


        public EnvClassTagHelper(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
           
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var envTitle = ApplicationService.AppConfigs.EnvironmentTitle;
            if (string.IsNullOrWhiteSpace(envTitle))
                output.AddClass(GetEnvClass("production"), HtmlEncoder.Default);
            else
                output.AddClass(GetEnvClass(envTitle.ToLower().Trim()), HtmlEncoder.Default);

            base.Process(context, output);
        }

        private string GetEnvClass(string envName)
        {
            if (Environments.ContainsKey(envName))
                return Environments[envName];
            else if (Environments.ContainsKey("all"))
                return Environments["all"];
            return string.Empty;
        }

    }
}