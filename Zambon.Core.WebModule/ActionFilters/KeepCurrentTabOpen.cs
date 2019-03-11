using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class KeepCurrentTabOpenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentTab = context.HttpContext.Request.Form["CurrentTabId"].ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(currentTab))
                if (context.Controller is Controller controller)
                    controller.TempData["CurrentTabId"] = currentTab;
            base.OnActionExecuting(context);
        }
    }
}