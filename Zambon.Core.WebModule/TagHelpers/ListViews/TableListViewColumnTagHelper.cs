using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;

namespace Zambon.Core.WebModule.TagHelpers.ListViews
{
    [HtmlTargetElement("tr", Attributes = ForAttributeName)]
    public class TableListViewColumnTagHelper : TagHelper
    {
        private const string ForAttributeName = "listview-column-for";

        #region Properties

        [HtmlAttributeName(ForAttributeName)]
        public Column[] For { get; set; }
        
        [HtmlAttributeName("listview-column-issublistview")]
        public string IsSubListView { get; set; }


        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IUrlHelperFactory UrlHelperFactory { get; }

        protected IUrlHelper UrlHelper { get { return UrlHelperFactory.GetUrlHelper(ViewContext); } }

        protected ApplicationService ApplicationService { get; }

        protected ExpressionsService ExpressionsService { get; }

        #endregion

        #region Constructors

        public TableListViewColumnTagHelper(IUrlHelperFactory urlHelperFactory, ApplicationService applicationService, ExpressionsService expressionsService)
        {
            UrlHelperFactory = urlHelperFactory;
            ApplicationService = applicationService;
            ExpressionsService = expressionsService;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((For?.Length ?? 0) > 0 && ViewContext.ViewData.Model is ListView listView)
            {
                for (var c = 0; c < For.Length; c++)
                {
                    var column = For[c];
                    if (For[c].Index >= 0)
                    {
                        var size = !string.IsNullOrWhiteSpace(column.Size) ? "-" + column.Size.ToLower().Trim() : string.Empty;
                        var truncate = size.ToLower() != "-fit" ? "text-truncate" : "text-nowrap";

                        string alignment = null;
                        if (!string.IsNullOrWhiteSpace(column.Alignment))
                            switch(column.Alignment.ToLower().Trim())
                            {
                                case "center": case "right":
                                    alignment = column.Alignment.ToLower().Trim();
                                    break;
                            }
                            

                        var foreColor = "text-body";
                        if (listView.GetApplicableForeColor(ExpressionsService, listView.CurrentObject) is string customForeColor && !string.IsNullOrWhiteSpace(customForeColor))
                            foreColor = customForeColor;

                        var customClass = listView.GetApplicableCssClass(ExpressionsService, listView.CurrentObject);

                        var wrapper = string.Empty;
                        if ((listView.CanEdit ?? false) && !string.IsNullOrWhiteSpace(listView.ControllerName) && !string.IsNullOrWhiteSpace(listView.ActionName) && !string.IsNullOrWhiteSpace(listView.EditModalId))
                        {
                            var onClick = string.Empty;
                            if (!string.IsNullOrWhiteSpace(IsSubListView) && IsSubListView.ToUpper() == "TRUE")
                            {
                                var view = ApplicationService.GetDetailView(ViewContext.TempData["CurrentViewID"].ToString());
                                onClick = $"onclick=\"return ajax_update_parent('{UrlHelper.Action("UpdateData", view.ControllerName)}',this);\"";
                            }

                            wrapper = $"<a href=\"{GetLinkAction()}\" class=\"{truncate} {foreColor} {customClass}\" " +
                                $"data-ajax=\"true\" data-ajax-method=\"POST\" data-ajax-mode=\"replace\" data-ajax-begin=\"AjaxBegin\" data-ajax-failure=\"AjaxFailure\" data-ajax-success=\"AjaxSuccess\" data-ajax-complete=\"AjaxComplete\" data-ajax-update-loading=\"body\" " +
                                $"data-ajax-update=\"#{listView.EditModalId}_container\" data-ajax-open-modal=\"#{listView.EditModalId}\" {onClick}>{{0}}</a>";
                        }
                        else
                            wrapper = $"<span class=\"{truncate} {foreColor} {customClass}\">{{0}}</span>";


                        var cellValue = listView.GetCellValue(ApplicationService, column);

                        var content = string.Empty;
                        if (cellValue == null || (cellValue is string && string.IsNullOrWhiteSpace(cellValue.ToString())))
                            content = "<span>&nbsp;</span>";
                        else if (cellValue is bool)
                        {
                            if (alignment == null)
                                alignment = "center";

                            content = $"<input type=\"checkbox\" {((bool)cellValue ? "checked=checked " : "")} disabled />";
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

        private string GetLinkAction()
        {
            if (ViewContext.ViewData.Model is ListView listView)
            {
                IDictionary<string, string> parameters;
                if (!string.IsNullOrWhiteSpace(listView.EditModalParameters))
                    parameters = ExpressionsService.FormatActionParameters(listView.EditModalParameters, listView.CurrentObject);
                else
                    parameters = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(IsSubListView) && IsSubListView.ToUpper() == "TRUE")
                    parameters.Add("ParentViewId", ViewContext.TempData["CurrentViewID"].ToString());
                else
                    parameters.Add("ParentViewId", listView.ViewId);

                parameters.Add("ModalId", listView.EditModalId);
                parameters.Add("ViewId", listView.EditModal.ViewId);

                return UrlHelper.Action(listView.ActionName, listView.ControllerName, parameters);
            }
            return string.Empty;
        }

    }
}