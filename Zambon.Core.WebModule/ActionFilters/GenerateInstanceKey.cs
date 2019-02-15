﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class GenerateInstanceKey : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Cookies["InstanceKey"] == null)
                context.HttpContext.Response.Cookies.Append("InstanceKey", Guid.NewGuid().ToString());
            base.OnActionExecuting(context);
        }

    }
}