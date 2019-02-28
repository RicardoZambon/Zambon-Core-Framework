using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.ViewComponents
{
    public class SubListViewViewComponent : ViewComponent
    {

        private readonly ApplicationService App;

        private readonly CoreDbContext Ctx;

        public SubListViewViewComponent(ApplicationService app, CoreDbContext ctx)
        {
            App = app;
            Ctx = ctx;
        }

        public IViewComponentResult Invoke(object entity, IEnumerable collection, string subListViewId)
        {
            var listView = App.GetListView(subListViewId);

            listView.SetItemsCollection(App, Ctx, collection);
            listView.SetCurrentObject(entity);

            return View(listView);
        }
    }
}