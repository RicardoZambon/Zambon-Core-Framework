using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.SubViews;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Text.Encodings.Web;

namespace Zambon.Core.WebModule.TagHelpers.ListViews
{
    [HtmlTargetElement("tbody", Attributes = ForAttributeName)]
    public class TableListViewBodyTagHelper : TagHelper
    {
        private const string ForAttributeName = "listview-body-for";

        public override int Order => -9001;

        [HtmlAttributeName(ForAttributeName)]
        public Column[] For { get; set; }

        [HtmlAttributeName("listview-body-issublistview")]
        public string IsSubListView { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }


        protected IUrlHelperFactory UrlHelperFactory { get; }

        protected ApplicationService ApplicationService { get; }

        protected ExpressionsService ExpressionsService { get; }


        public TableListViewBodyTagHelper(IUrlHelperFactory urlHelperFactory, ApplicationService applicationService, ExpressionsService expressionsService)
        {
            UrlHelperFactory = urlHelperFactory;
            ApplicationService = applicationService;
            ExpressionsService = expressionsService;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.ViewData.Model is ListView listView)
            {
                if ((listView.ItemsCollection?.Count() ?? 0) > 0)
                {
                    foreach (var currentObject in listView.ItemsCollection)
                    {
                        listView.SetCurrentObject(currentObject);

                        var rowOutput = CreateTagHelperOutput("tr");
                        rowOutput.Content.AppendHtml("<th scope=\"row\" class=\"col-0 pl-0 pr-0\">&nbsp;</th>");

                        //Buttons
                        if((listView.Buttons?.Length ?? 0) > 0 && listView.Buttons.Any(x => x.IsApplicable("Inline")))
                        {
                            rowOutput.Content.AppendHtml("<th scope=\"row\" class=\"col-fit text-center\">");

                            var buttonTagHelper = new ButtonTagHelper(UrlHelperFactory, ApplicationService, ExpressionsService) { For = listView.Buttons, Target="Inline", _HideText = "true", CurrentObject = currentObject, ViewContext = ViewContext };
                            var buttonOutput = CreateTagHelperOutput("div");
                            buttonTagHelper.Process(context, buttonOutput);
                            rowOutput.Content.AppendHtml(buttonOutput.Content.GetContent());

                            rowOutput.Content.AppendHtml("</th>");
                        }

                        //Columns
                        var columnTagHelper = new TableListViewColumnTagHelper(UrlHelperFactory, ApplicationService, ExpressionsService) { For = listView.Columns, IsSubListView = IsSubListView, ViewContext = ViewContext };
                        columnTagHelper.Process(context, rowOutput);


                        var backColor = listView.GetApplicableBackColor(ExpressionsService, currentObject);

                        output.Content.AppendHtml($"<tr class=\"{backColor}\">{rowOutput.Content.GetContent() }</tr>");
                    }
                    listView.ClearCurrentObject();
                }
                else if (!string.IsNullOrWhiteSpace(listView.MessageOnEmpty))
                {
                    output.Content.AppendHtml(
                    $"<tr>" +
                        $"<td class=\"col text-center\" colspan=\"{1 + ((listView?.Buttons?.Length ?? 0) > 0 && listView.Buttons.Any(x => x.IsApplicable("Inline")) ? 1 : 0) + (listView?.Columns?.Length ?? 0)}\">{listView.MessageOnEmpty}</td>" +
                    "</tr>");
                }
            }
        }

        private TagHelperOutput CreateTagHelperOutput(string tagName)
        {
            return new TagHelperOutput(
                tagName: tagName,
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (s, t) =>
                {
                    return Task.Factory.StartNew<TagHelperContent>(
                            () => new DefaultTagHelperContent());
                }
            );
        }

    }
}