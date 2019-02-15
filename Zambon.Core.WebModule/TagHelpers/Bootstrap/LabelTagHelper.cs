using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Zambon.Core.WebModule.TagHelpers.Bootstrap
{
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class LabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        #region Properties

        public override int Order => 0;

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        #endregion

        #region Constructors

        public LabelTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            string extractedClassValue;
            switch (output.Attributes["class"]?.Value ?? string.Empty)
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

            if (!extractedClassValue.Contains("control-label") || !extractedClassValue.Contains("custom-control-label"))
            {
                output.AddClass("control-label", HtmlEncoder.Default);
                output.AddClass("mb-0", HtmlEncoder.Default);
            }
        }

    }
}