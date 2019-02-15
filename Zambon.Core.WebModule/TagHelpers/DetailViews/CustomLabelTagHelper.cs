using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class CustomLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        public override int Order => -1001;

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        protected ApplicationService Application { get; }


        public CustomLabelTagHelper(IHtmlGenerator generator, ApplicationService application)
        {
            Generator = generator;
            Application = application;
        }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var tagBuilder = Generator.GenerateLabel(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                labelText: null,
                htmlAttributes: null);

            if (tagBuilder != null)
            {
                output.MergeAttributes(tagBuilder);

                // Do not update the content if another tag helper targeting this element has already done so.
                if (!output.IsContentModified)
                {
                    // We check for whitespace to detect scenarios such as:
                    // <label for="Name">
                    // </label>
                    var childContent = await output.GetChildContentAsync();
                    if (childContent.IsEmptyOrWhiteSpace)
                    {
                        //var displayName = Model.GetPropertyDisplayName(Zambon.Core.Database.Tracking.CustomTracker.GetCorrectType(For.ModelExplorer.Container.ModelType).FullName, For.Name);
                        var displayName = Application.GetPropertyDisplayName(For.Metadata.ContainerMetadata.ModelType.FullName, For.Name);

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