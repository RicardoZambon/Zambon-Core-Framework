using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.DetailViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class ParameterAsDictionaryAttribute : ActionFilterAttribute
    {

        public string ArgumentKey { get; set; }

        public string Parameters { get; set; }

        public string Ignore { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var arrParameters = Parameters.Split(",");
            var arrIgnore = Ignore.Split(",");

            var ListOfParameters = new Dictionary<string, object>();

            foreach (var argument in context.HttpContext.Request.Query)
                if (!arrIgnore.Contains(argument.Key))
                    if (arrParameters.Contains(argument.Key) || arrParameters.Contains("*"))
                        ListOfParameters.Add(argument.Key, argument.Value.ToString());

            var key = string.IsNullOrWhiteSpace(ArgumentKey) ? "parameters" : ArgumentKey;

            if (context.ActionArguments.ContainsKey(key))
                context.ActionArguments[key] = ListOfParameters;
            else
                context.ActionArguments.Add(key, ListOfParameters);

            base.OnActionExecuting(context);
        }

    }
}