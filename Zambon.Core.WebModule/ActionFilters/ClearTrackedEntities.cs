using Microsoft.AspNetCore.Mvc.Filters;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.WebModule.Helper;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class ClearTrackedEntitiesAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.RequestServices.GetService(typeof(CoreDbContext)) is CoreDbContext ctx)
            {
                if (context.ActionArguments.ContainsKey("viewInfo") && context.ActionArguments["viewInfo"] is ViewInfo viewInfo && !string.IsNullOrWhiteSpace(viewInfo.ParentViewId)
                    && context.HttpContext.RequestServices.GetService(typeof(ApplicationService)) is ApplicationService app)
                {
                    var parentView = app.GetView(viewInfo.ParentViewId);
                    if (parentView is DetailView detailView)
                    {
                        var view = app.GetView(viewInfo.ViewId);
                        ctx.ClearTrackedEntities(clearStored: false, tempModelType: view?.GetEntityType()?.Name ?? string.Empty);
                        return;
                    }
                }
                ctx.ClearTrackedEntities(forceClear: true);
            }            
            base.OnActionExecuting(context);
        }

    }
}