using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("div", Attributes = ValidationSummaryAttributeName)]
    public class ValidatorSummaryTagHelper : TagHelper
    {
        private const string ValidationSummaryAttributeName = "asp-validation-summary";

        #region Properties

        /// <inheritdoc />
        public override int Order => 0;

        [HtmlAttributeName(ValidationSummaryAttributeName)]
        public ValidationSummary ValidationSummary { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        #endregion

        #region Constructors

        public ValidatorSummaryTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (output.Attributes["class"] != null)
            {
                string extractedClassValue;
                switch (output.Attributes["class"].Value)
                {
                    case string valueAsString:
                        extractedClassValue = HtmlEncoder.Default.Encode(valueAsString);
                        break;
                    case HtmlString valueAsHtmlString:
                        extractedClassValue = valueAsHtmlString.Value;
                        break;
                    case IHtmlContent htmlContent:
                        using (var stringWriter = new StringWriter())
                        {
                            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
                            extractedClassValue = stringWriter.ToString();
                        }
                        break;
                    default:
                        extractedClassValue = HtmlEncoder.Default.Encode(output.Attributes["class"].Value?.ToString());
                        break;
                }

                if (!string.IsNullOrEmpty(extractedClassValue) && extractedClassValue.Contains("validation-summary-errors"))
                {
                    output.RemoveClass("validation-summary-errors", HtmlEncoder.Default);
                    output.AddClass("card", HtmlEncoder.Default);
                    output.AddClass("bg-danger", HtmlEncoder.Default);
                    output.AddClass("text-white", HtmlEncoder.Default);
                    output.AddClass("col", HtmlEncoder.Default);
                    output.AddClass("mb-2", HtmlEncoder.Default);
                    output.AddClass("p-2", HtmlEncoder.Default);
                }
            }

            var postContent = output.PostContent?.GetContent(HtmlEncoder.Default) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(postContent))
                output.PostContent.SetHtmlContent(postContent.Replace("<ul>", "<ul class=\"mb-0 pl-4\">"));
        }

        #endregion

    }
}