using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;
using System.Text.Encodings.Web;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("input", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
    public class ValidatorInputTagHelper : TagHelper
    {

        #region Overrides

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
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