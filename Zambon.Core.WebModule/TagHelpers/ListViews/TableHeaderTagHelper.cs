using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.WebModule.TagHelpers.ListViews
{
    [HtmlTargetElement("thead", Attributes = ForAttributeName)]
    public class TableHeaderTagHelper : TagHelper
    {
        private const string ForAttributeName = "listview-header-for";

        #region Properties

        public override int Order => 0;

        [HtmlAttributeName(ForAttributeName)]
        public Column[] For { get; set; }

        [HtmlAttributeName("listview-header-class")]
        public string CustomClass { get; set; }


        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        #endregion

        #region Constructors

        public TableHeaderTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if ((For?.Length ?? 0) > 0)
            {
                output.Content.AppendHtml("<tr><th scope=\"col\" class=\"col-0 pl-0 pr-0\">&nbsp;</th>");

                switch(ViewContext.ViewData.Model)
                {
                    case ListView listView:
                        if ((listView.Buttons?.Length ?? 0) > 0 && listView.Buttons.Any(x => x.IsApplicable("Inline")))
                            output.Content.AppendHtml("<th scope=\"col\" class=\"col-fit text-center\"><span class=\"oi oi-command\"></span></th>");
                        break;
                    case LookupModal lookupModal:
                        output.Content.AppendHtml("<th scope=\"col\" class=\"col-fit text-center\"><span class=\"oi oi-command\"></span></th>");
                        break;
                }

                for (var c = 0; c < For.Length; c++)
                    if (For[c].Index >= 0)
                    {
                        var size = !string.IsNullOrWhiteSpace(For[c].Size) ? "-" + For[c].Size : string.Empty;
                        output.Content.AppendHtml($"<th scope=\"col\" class=\"col{size} {CustomClass}\"><span class=\"text-truncate\">{For[c].DisplayName}</span></th>");
                    }
                output.Content.AppendHtml($"</tr>");
            }
        }

    }
}