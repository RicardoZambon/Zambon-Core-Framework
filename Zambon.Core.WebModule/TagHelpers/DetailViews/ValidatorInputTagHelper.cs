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
    [HtmlTargetElement("input", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class ValidatorInputTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        #region Properties

        /// <inheritdoc />
        public override int Order => 0;

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        #endregion

        #region Constructors

        public ValidatorInputTagHelper(IHtmlGenerator generator)
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

                if (!string.IsNullOrEmpty(extractedClassValue) && extractedClassValue.Contains("input-validation-error"))
                {
                    output.RemoveClass("input-validation-error", HtmlEncoder.Default);
                    output.AddClass("is-invalid", HtmlEncoder.Default);
                }
            }
        }

        #endregion

    }
}