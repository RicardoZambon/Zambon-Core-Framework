using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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