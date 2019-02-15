using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Zambon.Core.WebModule.ActionFilters
{
    public class CustomModelValidator : IObjectModelValidator
    {
        public void Validate(ActionContext actionContext, ValidationStateDictionary validationState, string prefix, object model)
        {
            actionContext.ModelState.Clear();
        }
    }
}