using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;
using System.Text.Encodings.Web;

namespace Zambon.Core.WebModule.TagHelpers.Bootstrap
{
    [HtmlTargetElement("label", Attributes = "asp-for")]
    public class LabelTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
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