using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Zambon.Core.Database;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class MergeActionValuesFromDBAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor
                && context.HttpContext.RequestServices.GetService(typeof(CoreDbContext)) is CoreDbContext ctx)
            {
                var parameters = descriptor.MethodInfo.GetParameters();
                for (var i = 0; i < parameters.Length; i++)
                    if (context.ActionArguments.ContainsKey(parameters[i].Name) && context.ActionArguments[parameters[i].Name] is IKeyed model)
                    {
                        var formKeys = context.HttpContext.Request.Form.Keys.Where(x => x.IndexOf("RequestVerificationToken") < 0 && x.IndexOf("X-Requested-With") < 0).Select(x => x.Replace(parameters[i].Name, "").Replace("[", "").Replace("]", "")).ToList();
                        formKeys.AddRange(context.HttpContext.Request.Form.Files.Select(x => x.Name).Where(x => x.IndexOf(".") < 0));

                        //Replace the parameter in action event
                        context.ActionArguments.Remove(parameters[i].Name);

                        var entity = ctx.Merge(model, formKeys);
                        context.ActionArguments.Add(parameters[i].Name, entity);
                    }
            }
            base.OnActionExecuting(context);
        }
    }
}