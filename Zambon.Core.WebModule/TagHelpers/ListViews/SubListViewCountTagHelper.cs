using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Linq.Dynamic.Core;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.WebModule.TagHelpers.ListViews
{
    [HtmlTargetElement("span", Attributes = ForAttributeName)]
    public class SubListViewCountTagHelper : TagHelper
    {
        private const string ForAttributeName = "listview-count-for";

        #region Properties

        [HtmlAttributeName(ForAttributeName)]
        public string For { get; set; }
        
        [HtmlAttributeName("listview-collection")]
        public string Collection { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }


        protected ApplicationService ApplicationService { get; }

        protected CoreDbContext Ctx { get; set; }

        #endregion

        #region Constructors

        public SubListViewCountTagHelper(ApplicationService applicationService, CoreDbContext ctx)
        {
            ApplicationService = applicationService;
            Ctx = ctx;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ApplicationService.GetListView(For) is ListView listView)
            {
                listView.SetItemsCollection(ApplicationService, Ctx, ViewContext.ViewData.Model, Collection);
                output.Content.Append((listView.ItemsCollection?.Count() ?? 0).ToString());
            }
        }

    }
}