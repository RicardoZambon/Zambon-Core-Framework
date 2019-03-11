using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zambon.Core.Database.Interfaces;

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
                    if (context.ActionArguments[parameters[i].Name].GetType().GetProperties().Count(x => x.GetCustomAttributes(typeof(ValidationAttribute), true).Count() > 0) > 0
                        && context.ActionArguments[parameters[i].Name] is ICustomValidated entity)
                        entity.IsModelValid(context.HttpContext.RequestServices, context.ModelState);
            }
            base.OnActionExecuting(context);
        }
    }
}