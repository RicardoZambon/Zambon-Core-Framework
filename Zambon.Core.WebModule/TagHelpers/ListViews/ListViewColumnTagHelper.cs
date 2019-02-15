using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Database.Helper;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace Zambon.Core.WebModule.TagHelpers.ListViews
{
    [HtmlTargetElement("td", Attributes = ForAttributeName)]
    public class ListViewColumnTagHelper : TagHelper
    {
        private const string ForAttributeName = "listview-column-for";

        #region Properties

        public override int Order => 0;

        [HtmlAttributeName(ForAttributeName)]
        public Column For { get; set; }

        [HtmlAttributeName("listview-column-model")]
        public ListView Model { get; set; }

        [HtmlAttributeName("listview-column-item-class")]
        public string CustomClass { get; set; }

        [HtmlAttributeName("listview-column-item-forecolor")]
        public string ForeColor { get; set; }

        [HtmlAttributeName("listview-column-is-sublistview")]
        public string IsSubListView { get; set; }



        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        protected IUrlHelperFactory UrlHelperFactory { get; }

        protected IUrlHelper UrlHelper { get { return UrlHelperFactory.GetUrlHelper(ViewContext); } }

        protected ApplicationService Application { get; }

        protected GlobalExpressionsService Expressions { get; }

        #endregion

        #region Constructors

        public ListViewColumnTagHelper(IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory, ApplicationService application, GlobalExpressionsService expressions)
        {
            Generator = generator;
            UrlHelperFactory = urlHelperFactory;
            Application = application;
            Expressions = expressions;
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
                

                var foreColor = "text-body";
                if (!string.IsNullOrWhiteSpace(ForeColor))
                    foreColor = ForeColor;

                var truncate = string.Empty;
                if (size.ToLower() != "-fit")
                {
                    output.AddClass($"text-truncate", HtmlEncoder.Default);
                    truncate = "text-truncate";
                }
                else
                    output.Attributes.Add("style", "white-space: nowrap");

                var onClick = string.Empty;
                if (!string.IsNullOrWhiteSpace(IsSubListView) && IsSubListView.ToUpper() == "TRUE")
                {
                    var view = Application.GetDetailView(ViewContext.TempData["CurrentViewID"].ToString());
                    onClick = $"onclick=\"return ajax_update_parent('{UrlHelper.Action("UpdateData", view.ControllerName)}',this);\"";
                }

                var wrapper = string.Empty;
                if (Model.CanEdit && !string.IsNullOrWhiteSpace(Model.ControllerName) && !string.IsNullOrWhiteSpace(Model.ActionName) && !string.IsNullOrWhiteSpace(Model.EditModalId))
                    wrapper = $"<a href=\"{GetLinkAction()}\" class=\"{truncate} {foreColor} {CustomClass}\" " +
                        $"data-ajax=\"true\" data-ajax-method=\"POST\" data-ajax-mode=\"replace\" data-ajax-begin=\"AjaxBegin\" data-ajax-failure=\"AjaxFailure\" data-ajax-success=\"AjaxSuccess\" data-ajax-complete=\"AjaxComplete\" data-ajax-update-loading=\"body\" " +
                        $"data-ajax-update=\"#{Model.EditModalId}_container\" data-ajax-open-modal=\"#{Model.EditModalId}\" {onClick}>{{0}}</a>";
                else
                    wrapper = $"<span class=\"{truncate} text-body {CustomClass}\">{{0}}</span>";

                var cellValue = Model.GetCellValue(Application, For);

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

        private string GetLinkAction()
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(Model.EditModalParameters))
            {
                var currentObject = Application.GetListViewCurrentObject(Model.ViewId);
                if (currentObject == null)
                    parameters = Model.EditModalParameters.Split('&').ToDictionary(k => k.Split('=')[0], v => v.Split('=')[1]);
                else
                {
                    var method = typeof(GlobalExpressionsService).GetMethod("FormatExpressionTypedValue").MakeGenericMethod(currentObject.GetType().GetCorrectType());
                    parameters = Model.EditModalParameters.Split('&').ToDictionary(k => k.Split('=')[0], v => method.Invoke(Expressions, new object[] { v.Split('=')[1], currentObject }).ToString());
                }
            }

            if (!string.IsNullOrWhiteSpace(IsSubListView) && IsSubListView.ToUpper() == "TRUE")
                parameters.Add("ParentViewId", ViewContext.TempData["CurrentViewID"].ToString());
            else
                parameters.Add("ParentViewId", Model.ViewId);

            parameters.Add("ModalId", Model.EditModalId);
            parameters.Add("ViewId", Model.EditModal.ViewId);

            return UrlHelper.Action(Model.ActionName, Model.ControllerName, parameters);
        }

    }
}