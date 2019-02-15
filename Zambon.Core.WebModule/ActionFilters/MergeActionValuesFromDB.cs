using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.Operations;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Zambon.Core.WebModule.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class MergeActionValuesFromDBAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor
                && context.HttpContext.RequestServices.GetService(typeof(CoreContext)) is CoreContext ctx)
            {
                var parameters = descriptor.MethodInfo.GetParameters();
                for (var i = 0; i < parameters.Length; i++)
                    if (context.ActionArguments.ContainsKey(parameters[i].Name) && context.ActionArguments[parameters[i].Name] is BaseDBObject model)
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