using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.WebModule.TagHelpers.LookupViews
{
    [HtmlTargetElement("tr", Attributes = ForAttributeName)]
    public class TableLookupViewColumnTagHelper : TagHelper
    {
        private const string ForAttributeName = "lookupview-column-for";

        #region Properties

        [HtmlAttributeName(ForAttributeName)]
        public Column[] For { get; set; }


        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly ApplicationService ApplicationService;

        #endregion

        #region Constructors

        public TableLookupViewColumnTagHelper(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((For?.Length ?? 0) > 0 && ViewContext.ViewData.Model is LookupModal lookupModal)
                for (var c = 0; c < For.Length; c++)
                {
                    var column = For[c];
                    if (For[c].Index >= 0)
                    {
                        var size = !string.IsNullOrWhiteSpace(column.Size) ? "-" + column.Size.ToLower().Trim() : string.Empty;
                        var truncate = size.ToLower() != "-fit" ? "text-truncate" : "text-nowrap";

                        string alignment = null;
                        if (!string.IsNullOrWhiteSpace(column.Alignment))
                            switch (column.Alignment.ToLower().Trim())
                            {
                                case "center":
                                case "right":
                                    alignment = column.Alignment.ToLower().Trim();
                                    break;
                            }

                        var wrapper = $"<span class=\"{truncate} text-body\">{{0}}</span>";
                        var cellValue = lookupModal.LookUpView.GetCellValue(ApplicationService, column);

                        var content = string.Empty;
                        if (cellValue == null || (cellValue is string && string.IsNullOrWhiteSpace(cellValue.ToString())))
                            content = "<span>&nbsp;</span>";
                        else if (cellValue is bool)
                        {
                            if (alignment == null)
                                alignment = "center";

                            content = $"<input type=\"checkbox\" {((bool)cellValue ? "checked=checked " : "")} class=\"align-middle\" disabled />";
                        }
                        else
                            content = cellValue.ToString();

                        output.Content.AppendHtml($"<td class=\"col{size} text-{alignment ?? "left"}\" >");
                        output.Content.AppendHtml(wrapper.Replace("{0}", content));
                        output.Content.AppendHtml("</td>");
                    }
                }
        }

    }
}