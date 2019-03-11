using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Zambon.Core.Database;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class LoadTrackedEntities : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor
                && context.HttpContext.RequestServices.GetService(typeof(CoreDbContext)) is CoreDbContext ctx)
                ctx.LoadTrackedEntities();
            base.OnActionExecuting(context);
        }
    }
}