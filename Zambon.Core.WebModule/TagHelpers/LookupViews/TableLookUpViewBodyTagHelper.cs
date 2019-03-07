using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.WebModule.TagHelpers.LookupViews
{
    [HtmlTargetElement("tbody", Attributes = ForAttributeName)]
    public class TableLookupViewBodyTagHelper : TagHelper
    {
        private const string ForAttributeName = "lookupview-body-for";

        [HtmlAttributeName(ForAttributeName)]
        public Column[] For { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }


        protected ApplicationService ApplicationService { get; }


        public TableLookupViewBodyTagHelper(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.ViewData.Model is LookupModal lookupModal)
            {
                if ((lookupModal.LookUpView.ItemsCollection?.Count() ?? 0) > 0)
                {
                    foreach (var currentObject in lookupModal.LookUpView.ItemsCollection)
                    {
                        lookupModal.LookUpView.SetCurrentObject(currentObject);

                        var rowOutput = CreateTagHelperOutput("tr");
                        rowOutput.Content.AppendHtml("<th scope=\"row\" class=\"col-0 pl-0 pr-0\">&nbsp;</th>");

                        var id = 0;
                        if (currentObject is IKeyed keyedObject)
                        {
                            id = keyedObject.ID;
                        }

                        //Selection
                        rowOutput.Content.AppendHtml(
                        "<th scope=\"row\" class=\"col-fit text-center\">" +
                            "<div class=\"custom-control custom-checkbox lookupview\">" +
                                $"<input type=\"checkbox\" class=\"custom-control-input\" id=\"{lookupModal.Id}_table_item{id}\" name=\"LookupSelection\" value=\"{id}\">" +
                                $"<label class=\"custom-control-label\" for=\"{lookupModal.Id}_table_item{id}\">&nbsp;</label>" +
                            "</div>" +
                        "</th>");

                        //Columns
                        var columnTagHelper = new TableLookupViewColumnTagHelper(ApplicationService) { For = lookupModal.LookUpView.Columns, ViewContext = ViewContext };
                        columnTagHelper.Process(context, rowOutput);

                        output.Content.AppendHtml($"<tr id=\"{lookupModal.Id}_table_tr{id}\">{rowOutput.Content.GetContent() }</tr>");
                    }
                    lookupModal.LookUpView.ClearCurrentObject();
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