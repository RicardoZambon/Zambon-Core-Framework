using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class CustomLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        #region Properties

        public override int Order => -1001;

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        protected ApplicationService ApplicationService { get; }

        #endregion

        #region Constructors

        public CustomLabelTagHelper(IHtmlGenerator generator, ApplicationService application)
        {
            Generator = generator;
            ApplicationService = application;
        }

        #endregion

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = Generator.GenerateLabel(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                labelText: null,
                htmlAttributes: null);

            if (tagBuilder != null)
            {
                output.MergeAttributes(tagBuilder);
                if (!output.IsContentModified)
                {
                    var childContent = await output.GetChildContentAsync();
                    if (childContent.IsEmptyOrWhiteSpace)
                    {
                        var displayName = ApplicationService.GetPropertyDisplayName(For.Metadata.ContainerMetadata.ModelType.FullName, For.Name);
                        if (!string.IsNullOrWhiteSpace(displayName))
                            output.Content.SetHtmlContent(displayName);
                    }
                    else
                        output.Content.SetHtmlContent(childContent);
                }
            }
        }

    }
}