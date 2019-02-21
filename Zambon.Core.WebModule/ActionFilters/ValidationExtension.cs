using Microsoft.AspNetCore.Mvc.ModelBinding;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Database.ExtensionMethods;

namespace Zambon.Core.WebModule.ActionFilters
{
    public static class ValidationExtension
    {

        public static bool IsModelValid<T>(this T model, IServiceProvider serviceProvider, ModelStateDictionary modelState) where T : class, ICustomValidated
        {
            //modelState.Clear();
            
            var vc = new ValidationContext(model, serviceProvider, new Dictionary<object, object>());
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, vc, results, true))
            {
                foreach (var result in results)
                {
                    if (result.MemberNames.Count() > 0)
                        foreach (var property in result.MemberNames)
                            modelState.AddModelError(property, result.ErrorMessage);
                    else
                        modelState.AddModelError("*", result.ErrorMessage);
                }
            }
            else
                if (serviceProvider.GetService(typeof(CoreDbContext)) is CoreDbContext ctx && model is IDBObject entity)
                    if (!entity.IsValid(ctx, out List<KeyValuePair<string, string>> errors))
                        for (var i = 0; i < errors.Count; i++)
                            modelState.AddModelError(errors[i].Key, errors[i].Value);

            return modelState.ErrorCount == 0;
        }

    }
}