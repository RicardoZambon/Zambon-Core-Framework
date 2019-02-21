using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Navigation;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.Buttons;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.SubViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("div", Attributes = ForAttributeName)]
    public class ButtonTagHelper : TagHelper
    {
        private const string ForAttributeName = "button-for";

        #region Properties

        public override int Order => -5001;

        [HtmlAttributeName(ForAttributeName)]
        public Button[] For { get; set; }

        [HtmlAttributeName("button-target")]
        public string Target { get; set; }

        [HtmlAttributeName("button-hide-text")]
        public string _HideText { get; set; }

        public bool HideText { get { return (_HideText?.ToString()?.ToUpper() ?? "FALSE") == "TRUE"; } }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName("button-current-object")]
        public object CurrentObject { get; set; }

        [HtmlAttributeName("button-class")]
        public string ButtonClass { get; set; }


        public View CurrentView { get { return Application.GetView(ViewContext.TempData["CurrentViewID"]?.ToString() ?? string.Empty); } }


        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        protected IUrlHelperFactory UrlHelperFactory { get; }

        protected IUrlHelper UrlHelper { get { return UrlHelperFactory.GetUrlHelper(ViewContext); } }

        protected ApplicationService Application { get; }

        protected GlobalExpressionsService Expressions { get; }

        #endregion

        #region Constructors

        public ButtonTagHelper(IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory, ApplicationService application, GlobalExpressionsService expressions)
        {
            Generator = generator;
            UrlHelperFactory = urlHelperFactory;
            Application = application;
            Expressions = expressions;
        }

        #endregion

        #region Overrides

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (Target.StartsWith("ListView_"))
                SubListViewToolbarButtons(output);
            else
            {
                switch (Target)
                {
                    case "Toolbar":
                        ToolbarButtons(output);
                        break;
                    case "Inline":
                        InlineButtons(output);
                        break;
                    //case "ModalFooter":
                    //    output.Content.AppendHtml(await output.GetChildContentAsync());
                    //    CustomLocationButtons(output);
                    //    break;
                    default:
                        output.Content.AppendHtml(await output.GetChildContentAsync());
                        CustomLocationButtons(output);
                        break;
                }
            }
        }

        #endregion

        #region Toolbar

        private void ToolbarButtons(TagHelperOutput output)
        {
            for (var i = 0; i < For.Length; i++)
            {
                var button = For[i];
                if (button.IsApplicable("Toolbar", null, Expressions))
                {
                    var tag = string.Empty;
                    if ((button.SubButtons?.Button?.Length ?? 0) == 0)
                        tag = CreateToolbarItem(button);
                    else
                        tag = CreateToolbarParents(button);
                    output.Content.AppendHtml(tag);
                }
            }
        }

        private string CreateToolbarParents(Button button)
        {
            var icon = string.Empty;
            if (!string.IsNullOrWhiteSpace(button.IconName))
                icon = $"<span class=\"oi {button.IconName}\"></span>";

            var anchor = $"<a class=\"btn {button.ClassName} dropdown-toggle {ButtonClass}\" data-toggle=\"dropdown\" title=\"{button.DisplayName}\" href>{icon}{(!HideText ? button.DisplayName : string.Empty)}</a>";
            if ((button.SubButtons?.Button?.Length ?? 0) > 0)
            {
                anchor += "<div class=\"dropdown-menu dropdown-menu-right\">{0}</div>";
                for (var u = 0; u < button.SubButtons.Button.Length; u++)
                {
                    if (string.IsNullOrWhiteSpace(button.SubButtons.Button[u].ConditionExpression) || Expressions.IsApplicableItem(button.SubButtons.Button[u], new object()))
                        anchor = anchor.Replace("{0}", CreateToolbarItem(button.SubButtons.Button[u], true) + "{0}");
                }
                anchor = anchor.Replace("{0}", "");
            }

            return $"<div class=\"dropdown\">{anchor}</div>";
        }

        private string CreateToolbarItem(Button button, bool isDropDown = false)
        {
            var ajaxLoadingContainer = string.Empty;
            if (!string.IsNullOrWhiteSpace(button.LoadingContainer))
                ajaxLoadingContainer = $"data-ajax-update-loading=\"{button.LoadingContainer}\"";

            var ajaxConfirm = string.Empty;
            if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                ajaxConfirm = $"data-ajax-confirm=\"{button.ConfirmMessage}\"";

            var ajaxOpenModal = string.Empty;
            if (!string.IsNullOrWhiteSpace(button.OpenModal))
                ajaxOpenModal = $"data-ajax-mode=\"replace\" data-ajax-update=\"#{button.OpenModal}_container\" data-ajax-open-modal=\"#{button.OpenModal}\"";

            var icon = string.Empty;
            if (!string.IsNullOrWhiteSpace(button.IconName))
                icon = $"<span class=\"oi {button.IconName} mr-1\"></span>";

            return $"<a href=\"{GetButtonAction(button)}\" class=\"{(isDropDown ? "dropdown-item" : "btn")} {button.ClassName} {ButtonClass}\" " +
                $"data-ajax=\"true\" data-ajax-method=\"POST\" data-ajax-begin=\"AjaxBegin\" data-ajax-failure=\"AjaxFailure\" data-ajax-complete=\"AjaxComplete\" data-ajax-success=\"AjaxSuccess\" " +
                $"{ajaxLoadingContainer} {ajaxConfirm} {ajaxOpenModal} title=\"{button.DisplayName}\">{icon}{button.DisplayName}</a>";
        }

        #endregion

        #region Inline

        private void InlineButtons(TagHelperOutput output)
        {
            output.TagName = "span";
            output.AddClass("text-truncate", NullHtmlEncoder.Default);
            var index = 0;
            for (var i = 0; i < For.Length; i++)
            {
                var button = For[i];
                if (button.IsApplicable("Inline", CurrentObject, Expressions))
                {
                    output.Content.AppendHtml(CreateInlineItems(button, For.Length > 1));
                    index++;
                }
            }
        }

        private string CreateInlineItems(Button button, bool addMargin = false)
        {
            var buttonMethod = "";
            if (button.ActionMethod == "GET")
            {
                buttonMethod = "target=\"_blank\"";

                if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                    buttonMethod += $" onclick=\"return confirm('{button.ConfirmMessage}');\"";
            }
            else if (button.UseFormPost && string.IsNullOrEmpty(button.OpenModal))
            {
                buttonMethod = $"data-form-post=\"true\"";

                if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                    buttonMethod += $" onclick=\"return confirm('{button.ConfirmMessage}');\"";
            }
            else
            {
                buttonMethod = "data-ajax=\"true\" data-ajax-method=\"POST\" data-ajax-begin=\"AjaxBegin\" data-ajax-failure=\"AjaxFailure\" data-ajax-complete=\"AjaxComplete\" data-ajax-success=\"AjaxSuccess\"";

                if (!string.IsNullOrWhiteSpace(button.LoadingContainer))
                    buttonMethod += $" data-ajax-update-loading=\"{button.LoadingContainer}\"";

                if (!string.IsNullOrWhiteSpace(button.OpenModal))
                    buttonMethod += $" data-ajax-mode=\"replace\" data-ajax-update=\"#{button.OpenModal}_container\" data-ajax-open-modal=\"#{button.OpenModal}\"";

                if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                    buttonMethod += $" data-ajax-confirm=\"{button.ConfirmMessage}\"";
            }

            var icon = string.Empty;
            if (!string.IsNullOrWhiteSpace(button.IconName))
                icon = $"<span class=\"oi {button.IconName}\"></span>";

            var margin = addMargin ? "mr-1" : "";

            return $"<a href=\"{GetButtonAction(button)}\" class=\"{button.ClassName} {ButtonClass} {margin}\" " +
                $"{buttonMethod} title=\"{button.DisplayName}\">{icon}{(!HideText ? button.DisplayName : string.Empty)}</a>";
        }

        #endregion

        #region SublistView - Toolbar

        private void SubListViewToolbarButtons(TagHelperOutput output)
        {
            if ((For?.Length ?? 0) == 0)
            {
                output.TagMode = TagMode.SelfClosing;
                output.TagName = "";
                return;
            }

            for (var i = 0; i < For.Length; i++)
            {
                var button = For[i];
                if (button.IsApplicable("Toolbar", Application.GetDetailViewCurrentObject(CurrentView.ViewId), Expressions))
                    output.Content.AppendHtml(CreateSubListViewToolbarItems(button));
            }
            
            if ((CurrentView?.Buttons?.Button?.Length ?? 0) > 0)
                for (var i = 0; i < CurrentView.Buttons.Button.Length; i++)
                {
                    var button = CurrentView.Buttons.Button[i];
                    if (button.IsApplicable(Target, CurrentObject, Expressions))
                        output.Content.AppendHtml(CreateSubListViewToolbarItems(button));
                }
        }

        private string CreateSubListViewToolbarItems(Button button)
        {
            var action = string.Empty;
            var buttonMethod = string.Empty;
            if (button.ActionMethod == "GET")
            {
                buttonMethod = "target=\"_blank\"";
                action = GetButtonAction(button);

                if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                    buttonMethod += $" onclick=\"return confirm('{button.ConfirmMessage}');\"";
            }
            else if (button.UseFormPost && string.IsNullOrEmpty(button.OpenModal))
            {
                buttonMethod = $"data-form-post=\"true\"";
                action = GetButtonAction(button);

                if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                    buttonMethod += $" onclick=\"return confirm('{button.ConfirmMessage}');\"";
            }
            else
            {
                buttonMethod = "data-ajax=\"true\" data-ajax-method=\"POST\" data-ajax-begin=\"AjaxBegin\" data-ajax-failure=\"AjaxFailure\" data-ajax-complete=\"AjaxComplete\" data-ajax-success=\"AjaxSuccess\"";

                if (!string.IsNullOrWhiteSpace(button.LoadingContainer))
                    buttonMethod += $" data-ajax-update-loading=\"{button.LoadingContainer}\"";

                if (!string.IsNullOrWhiteSpace(button.OpenModal))
                    buttonMethod += $" data-ajax-mode=\"replace\" data-ajax-update=\"#{button.OpenModal}_container\" data-ajax-open-modal=\"#{button.OpenModal}\"";


                var modal = CurrentView.GetSubView(button.OpenModal);
                if (modal is DetailModal detailModal)
                {
                    buttonMethod += $" onclick=\"return ajax_update_parent('{UrlHelper.Action("UpdateData", CurrentView.ControllerName)}',this);\"";
                    action = UrlHelper.Action(button.ActionName, button.ControllerName, new { ParentObjectId = ViewContext.TempData["CurrentObjectID"].ToString(), ParentViewId = CurrentView.ViewId, ModalId = modal.Id, modal.ViewId });
                }
                else if (modal is LookupModal lookupModal)
                    action = UrlHelper.Action("LookupView", "View", new { ParentViewId = CurrentView.ViewId, ModalId = modal.Id, modal.ViewId, PostBackActionName = UrlHelper.Action(button.ActionName, button.ControllerName), PostbackFormId = ViewContext.TempData["CurrentModalID"].ToString() });
                else
                    action = GetButtonAction(button);
            }

            var icon = string.Empty;
            if (!string.IsNullOrWhiteSpace(button.IconName))
                icon = $"<span class=\"oi mr-1 {button.IconName}\"></span>";

            return $"<a href=\"{action}\" class=\"btn text-white btn-sm {button.ClassName}\" {buttonMethod} title=\"{button.DisplayName}\">{icon}{button.DisplayName}</a>"; //ajax-modal-postback=\"true\"
        }

        #endregion

        #region Custom location buttons

        private void CustomLocationButtons(TagHelperOutput output)
        {
            var index = 0;
            var buttons = For ?? CurrentView.Buttons.Button;
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                if (CurrentObject != null && button.IsApplicable(Target, CurrentObject, Expressions))
                {
                    output.Content.AppendHtml(CreateCustomLocationItems(button, index == 0 ? "ml-2" : string.Empty));
                    index++;
                }
            }
        }

        private string CreateCustomLocationItems(Button button, string customClass = "")
        {
            if (!string.IsNullOrEmpty(button.ActionName) && (CurrentView.ControllerName != button.ControllerName || CurrentView.ActionName != button.ActionName))
            {
                var buttonMethod = string.Empty;
                if (button.ActionMethod == "GET")
                {
                    buttonMethod = "target=\"_blank\"";

                    if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                        buttonMethod += $" onclick=\"return confirm('{button.ConfirmMessage}');\"";
                }
                else if (button.UseFormPost && string.IsNullOrEmpty(button.OpenModal))
                {
                    buttonMethod = $"data-form-post=\"true\"";

                    if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
                        buttonMethod += $" onclick=\"return confirm('{button.ConfirmMessage}');\"";
                }
                else
                {
                    buttonMethod = "data-ajax=\"true\" data-ajax-method=\"POST\" data-ajax-begin=\"AjaxBegin\" data-ajax-failure=\"AjaxFailure\" data-ajax-complete=\"AjaxComplete\" data-ajax-success=\"AjaxSuccess\"";

                    if (!string.IsNullOrWhiteSpace(button.LoadingContainer))
                        buttonMethod += $" data-ajax-update-loading=\"{button.LoadingContainer}\"";

                    if (!string.IsNullOrWhiteSpace(button.OpenModal))
                        buttonMethod += $" data-ajax-mode=\"replace\" data-ajax-update=\"#{button.OpenModal}_container\" data-ajax-open-modal=\"#{button.OpenModal}\"";
                }

                var icon = string.Empty;
                if (!string.IsNullOrWhiteSpace(button.IconName))
                    icon = $"<span class=\"oi mr-1 {button.IconName}\"></span>";

                return $"<a id=\"{button.Id}\" href=\"{GetButtonAction(button)}\" {buttonMethod} class=\"btn {button.ClassName} {ButtonClass} {customClass} {icon}\" title=\"{button.DisplayName}\">{(!HideText ? button.DisplayName : string.Empty)}</a>";
            }
            else
            {
                var icon = string.Empty;
                if (!string.IsNullOrWhiteSpace(button.IconName))
                    icon = $"oi {button.IconName}";

                return $"<input id=\"{button.Id}\" type=\"submit\" class=\"btn {button.ClassName} {ButtonClass} {customClass} {icon}\" value=\"{(!HideText ? button.DisplayName : string.Empty)}\"></input>";
            }
        }

        #endregion

        //#region Custom location buttons

        //private void CustomLocationButtons(TagHelperOutput output)
        //{
        //    var index = 0;
        //    for (var i = 0; i < (CurrentView.Buttons?.Button?.Length ?? 0); i++)
        //    {
        //        var button = CurrentView.Buttons.Button[i];
        //        if (CurrentObject != null && button.IsApplicable(Target, CurrentObject, Expressions))
        //        {
        //            output.Content.AppendHtml(CreateCustomLocationItems(button));
        //            index++;
        //        }
        //    }
        //}

        //private string CreateCustomLocationItems(Button button)
        //{
        //    if (!string.IsNullOrEmpty(button.ActionName) && (CurrentView.ControllerName != button.ControllerName || CurrentView.ActionName != button.ActionName))
        //    {
        //        var ajax = string.Empty;
        //        var action = string.Empty;

        //        if (string.IsNullOrWhiteSpace(button.OpenModal))
        //        {
        //            ajax = "data-form-post=\"true\"";
        //            action = GetButtonAction(button);

        //            if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
        //                ajax += $" data-ajax-confirm=\"{button.ConfirmMessage}\"";
        //        }
        //        else
        //        {
        //            ajax = "data-ajax=\"true\" data-ajax-method=\"POST\" data-ajax-begin=\"AjaxBegin\" data-ajax-failure=\"AjaxFailure\" data-ajax-complete=\"AjaxComplete\" data-ajax-success=\"AjaxSuccess\" data-ajax-mode=\"replace\" ";

        //            if (!string.IsNullOrWhiteSpace(button.LoadingContainer))
        //                ajax += $" data-ajax-update-loading=\"{button.LoadingContainer}\"";

        //            if (!string.IsNullOrWhiteSpace(button.ConfirmMessage))
        //                ajax += $" data-ajax-confirm=\"{button.ConfirmMessage}\"";

        //            ajax += $" data-ajax-update=\"#{button.OpenModal}_container\" data-ajax-open-modal=\"#{button.OpenModal}\"";

        //            ajax += $" onclick=\"return ajax_update_parent('{UrlHelper.Action("UpdateData", CurrentView.ControllerName)}',this);\"";

        //            var modal = CurrentView.GetSubView(button.OpenModal);
        //            action = UrlHelper.Action(button.ActionName, button.ControllerName, new { ParentObjectId = ViewContext.TempData["CurrentObjectID"].ToString(), ParentViewId = CurrentView.ViewId, ModalId = modal.Id, modal.ViewId });
        //        }

        //        var icon = string.Empty;
        //        if (!string.IsNullOrWhiteSpace(button.IconName))
        //            icon = $"<span class=\"oi mr-1 {button.IconName}\"></span>";

        //        return $"<a id=\"{button.Id}\" href=\"{action}\" {ajax} class=\"btn {button.ClassName} {ButtonClass} {icon}\" title=\"{button.DisplayName}\">{(!HideText ? button.DisplayName : string.Empty)}</a>";
        //    }
        //    else
        //    {
        //        var icon = string.Empty;
        //        if (!string.IsNullOrWhiteSpace(button.IconName))
        //            icon = $"oi {button.IconName}";

        //        return $"<input id=\"{button.Id}\" type=\"submit\" class=\"btn {button.ClassName} {ButtonClass} {icon}\" value=\"{(!HideText ? button.DisplayName : string.Empty)}\"></input>";
        //    }
        //}

        //#endregion

        private string GetButtonAction(Button button)
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(button.ActionParameters))
            {
                if (CurrentObject == null)
                    parameters = button.ActionParameters.Split('&').ToDictionary(k => k.Split('=')[0], v => v.Split('=')[1]);
                else
                {
                    var method = typeof(GlobalExpressionsService).GetMethod("FormatExpressionTypedValue").MakeGenericMethod(CurrentObject.GetType().GetCorrectType());
                    parameters = button.ActionParameters.Split('&').ToDictionary(k => k.Split('=')[0], v => method.Invoke(Expressions, new object[] { v.Split('=')[1], CurrentObject }).ToString());
                }
            }

            if (RouteValues.Count() > 0)
                foreach (var route in RouteValues)
                    if (!parameters.ContainsKey(route.Key))
                        parameters.Add(route.Key, route.Value);

            if (!string.IsNullOrWhiteSpace(button.OpenModal))
            {
                parameters.Add("ParentViewId", CurrentView.ViewId);
                parameters.Add("ModalId", button.OpenModal);
                parameters.Add("ViewId", CurrentView.GetSubView(button.OpenModal).ViewId);

                if (!string.IsNullOrWhiteSpace(button.ModalTitle))
                    parameters.Add("ModalTitle", button.ModalTitle);
            }
            else
                parameters.Add("ViewId", CurrentView.ViewId);

            return UrlHelper.Action(button.ActionName, button.ControllerName, parameters);
        }

    }
}