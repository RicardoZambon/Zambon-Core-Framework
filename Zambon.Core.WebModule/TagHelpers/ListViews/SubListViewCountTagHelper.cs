using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using Zambon.Core.Database;

namespace Zambon.Core.WebModule.TagHelpers.ListViews
{
    [HtmlTargetElement("span", Attributes = ForAttributeName)]
    public class SubListViewCountTagHelper : TagHelper
    {
        private const string ForAttributeName = "sublistview-count-for";

        #region Properties

        [HtmlAttributeName(ForAttributeName)]
        public string For { get; set; }

        [HtmlAttributeName("sublistview-count-model")]
        public BaseDBObject Model { get; set; }

        [HtmlAttributeName("sublistview-count-collection")]
        public string Collection { get; set; }


        protected ApplicationService App { get; }

        protected CoreDbContext Ctx { get; set; }

        #endregion

        #region Constructors

        public SubListViewCountTagHelper(ApplicationService app, CoreDbContext ctx)
        {
            App = app;
            Ctx = ctx;
        }

        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (App.GetListView(For) is ListView listView)
            {
                //TODO: Review, maybe we can remove the listview name in string and use direct the object.
                listView.SetItemsCollection(App, Ctx, Model, Collection);

                var itemsCollection = listView.ItemsCollection;
                output.Content.Append((itemsCollection?.Count() ?? 0).ToString());
            }
        }

    }
}
