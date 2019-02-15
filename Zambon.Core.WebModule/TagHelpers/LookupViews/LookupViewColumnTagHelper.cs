using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Zambon.Core.WebModule.TagHelpers.ListViews
{
    [HtmlTargetElement("td", Attributes = ForAttributeName)]
    public class LookupViewColumnTagHelper : TagHelper
    {
        private const string ForAttributeName = "lookupview-column-for";

        #region Properties

        public override int Order => 0;

        [HtmlAttributeName(ForAttributeName)]
        public Column For { get; set; }

        [HtmlAttributeName("lookupview-column-model")]
        public LookupView Model { get; set; }

        [HtmlAttributeName("lookupview-column-item-class")]
        public string CustomClass { get; set; }


        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly ApplicationService App;

        #endregion

        #region Constructors

        public LookupViewColumnTagHelper(ApplicationService app)
        {
            App = app;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (For.Index < 0)
            {
                output.TagName = null;
                output.TagMode = TagMode.StartTagAndEndTag;
            }
            else
            {
                output.AddClass(For.RecordClass, HtmlEncoder.Default);

                var size = !string.IsNullOrWhiteSpace(For.Size) ? "-" + For.Size : string.Empty;
                output.AddClass($"col{size}", HtmlEncoder.Default);
                output.AddClass($"text-truncate", HtmlEncoder.Default);

                var wrapper = $"<span class=\"text-truncate text-body {CustomClass}\">{{0}}</span>";

                var cellValue = Model.GetCellValue(App, For);

                var content = string.Empty;
                if (cellValue == null || (cellValue is string && string.IsNullOrWhiteSpace(cellValue.ToString())))
                    content = "<span>&nbsp;</span>";
                else if (cellValue is bool)
                    content = $"<input type=\"checkbox\" {((bool)cellValue ? "checked=checked " : "")} disabled />";
                else
                    content = cellValue.ToString();

                output.Content.SetHtmlContent(wrapper.Replace("{0}", content));
            }
        }

    }
}