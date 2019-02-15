using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Zambon.Core.Database.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class ValidateActionOnSavingAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var parameters = descriptor.MethodInfo.GetParameters();
                for (var i = 0; i < parameters.Length; i++)
                {
                    if (context.ActionArguments[parameters[i].Name].GetType().GetProperties().Count(x => x.GetCustomAttributes(typeof(ValidationAttribute), true).Count() > 0) > 0)
                    {
                        context.ActionArguments[parameters[i].Name].IsModelValid(context.HttpContext.RequestServices, context.ModelState);
                    }

                    //if (context.ActionArguments.ContainsKey(parameters[i].Name) && context.ActionArguments[parameters[i].Name] is IDBObject model)
                    //    model.IsModelValid(context.HttpContext.RequestServices, context.ModelState);
                }
            }
            base.OnActionExecuting(context);
        }

    }
}