using Zambon.Core.Database;
using Zambon.Core.Database.Helper;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Zambon.Core.Module.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RuleRequiredAttribute : ValidationAttribute
    {

        public string Condition { get; set; }

        public string ElementId { get; set; }


        public RuleRequiredAttribute()
        {

        }

        public RuleRequiredAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public RuleRequiredAttribute(string errorMessage, string Condition) : base(errorMessage)
        {
            this.Condition = Condition;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Condition) || Expression.ConditionIsApplicable(Condition, validationContext.ObjectInstance))
            {
                if (value == null || (value is int intValue && intValue.Equals(0)) || (value is string stringValue && stringValue.Equals(string.Empty)))
                {
                    var defaultMessage = "The property '{0}' is mandatory.";
                    var displayName = validationContext.DisplayName;

                    if (validationContext.GetService(typeof(ApplicationService)) is ApplicationService app)
                    {
                        defaultMessage = app.GetStaticText("ValidationMessageDefault_RuleRequired");
                        var displayText = app.GetPropertyDisplayName(validationContext.ObjectType.GetCorrectType().FullName, validationContext.MemberName);
                        if (!string.IsNullOrWhiteSpace(displayText))
                            displayName = displayText;
                    }

                    return new ValidationResult(
                        (!string.IsNullOrWhiteSpace(ErrorMessage) ? ErrorMessage : string.Format(defaultMessage, displayName)),
                        (string.IsNullOrWhiteSpace(ElementId) ? new[] { validationContext.MemberName }  : ElementId.Split(",")));
                }
            }

            return ValidationResult.Success;
        }

    }

}