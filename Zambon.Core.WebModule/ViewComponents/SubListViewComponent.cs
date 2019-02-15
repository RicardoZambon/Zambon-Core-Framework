using Microsoft.AspNetCore.Mvc;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Services;
using Zambon.Core.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zambon.Core.WebModule.ViewComponents
{
    public class SubListViewViewComponent : ViewComponent
    {

        private readonly ApplicationService app;

        private readonly GlobalExpressionsService _expressions;


        public SubListViewViewComponent(ApplicationService _app, GlobalExpressionsService expressions)
        {
            app = _app;
            _expressions = expressions;
        }

        public IViewComponentResult Invoke(BaseDBObject entity, string property, string subListViewId, string scrollSize = "md")
        {
            var listView = app.GetListView(subListViewId);

            listView.SetParameter("scroll-size", scrollSize);

            app.ClearListViewCurrentObject(listView.ViewId);
            listView.SetItemsCollection(entity, app, property);
            app.SetListViewCurrentObject(listView.ViewId, entity);

            return View(listView);
        }
    }
}