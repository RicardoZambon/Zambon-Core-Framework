using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Navigation;
using Zambon.Core.Module.Xml.Views.Buttons;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("form", Attributes = ForAttributeName)]
    public class FormModalTagHelper : TagHelper
    {
        private const string ForAttributeName = "form-for";

        #region Properties

        public override int Order => -1001;

        [HtmlAttributeName(ForAttributeName)]
        public DetailView For { get; set; }

        [HtmlAttributeName("form-modalid")]
        public string ModalViewId { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }


        protected IHtmlGenerator Generator { get; }

        protected IUrlHelperFactory UrlHelperFactory { get; }

        protected IUrlHelper UrlHelper { get { return UrlHelperFactory.GetUrlHelper(ViewContext); } }

        #endregion

        #region Constructors

        public FormModalTagHelper(IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory)
        {
            Generator = generator;
            UrlHelperFactory = urlHelperFactory;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (string.IsNullOrWhiteSpace(For.ActionName))
            {
                output.TagMode = TagMode.SelfClosing;
                output.TagName = "";
                return;
            }

            output.Attributes.Add("id", For.ViewId);// ModalViewId);

            output.AddClass("form-horizontal", HtmlEncoder.Default);
            output.Attributes.Add("method", "POST");
            output.Attributes.Add("role", "form");
            output.Attributes.Add("cancel-action", UrlHelper.Action("Cancel", For.ControllerName));

            if (!string.IsNullOrWhiteSpace(For.ControllerName) && !string.IsNullOrWhiteSpace(For.ActionName))
            {
                if (For.FormEnctype == "multipart/form-data")
                {
                    output.Attributes.Add("enctype", "multipart/form-data");
                    output.Attributes.Add("data-postaction", UrlHelper.Action(For.ActionName, For.ControllerName));
                    output.Attributes.Add("data-postupdate", "#" + (!string.IsNullOrWhiteSpace(ModalViewId) ? ModalViewId : For.ViewId) + "_container");
                    output.Attributes.Add("onsubmit", "return FormUploadSubmit(this);");
                }
                else
                {
                    output.Attributes.Add("action", UrlHelper.Action(For.ActionName, For.ControllerName));
                    output.Attributes.Add("data-ajax", "true");
                    output.Attributes.Add("data-ajax-method", "POST");
                    output.Attributes.Add("data-ajax-begin", "AjaxBegin");
                    output.Attributes.Add("data-ajax-failure", "AjaxFailure");
                    output.Attributes.Add("data-ajax-complete", "AjaxComplete");
                    output.Attributes.Add("data-ajax-success", "AjaxSuccess");
                    output.Attributes.Add("data-ajax-mode", "replace");
                    output.Attributes.Add("data-ajax-update", "#" + (!string.IsNullOrWhiteSpace(ModalViewId) ? ModalViewId : For.ViewId) + "_container");

                    if (!string.IsNullOrWhiteSpace(ModalViewId))
                        output.Attributes.Add("data-ajax-update-loading", "#" + ModalViewId + "_modal .modal-content");
                }
            }
        }
    }
}