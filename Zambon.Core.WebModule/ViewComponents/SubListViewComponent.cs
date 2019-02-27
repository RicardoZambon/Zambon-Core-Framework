using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        public IViewComponentResult Invoke(object entity, IQueryable property, string subListViewId)//, string scrollSize = "md")
        {
            var listView = App.GetListView(subListViewId);

            App.ClearListViewCurrentObject(listView.ViewId);
            listView.SetItemsCollection(App, Ctx, property);
            App.SetListViewCurrentObject(listView.ViewId, entity);

            return View(listView);
        }
    }
}