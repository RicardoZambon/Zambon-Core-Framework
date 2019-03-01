using System;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Module.Services;

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
            var expressionService = validationContext.GetService(typeof(ExpressionsService)) as ExpressionsService;
            if (string.IsNullOrWhiteSpace(Condition) || expressionService == null || expressionService.IsConditionApplicable(Condition, validationContext.ObjectInstance))
            {
                if (value == null || (value is int intValue && intValue.Equals(0)) || (value is string stringValue && stringValue.Equals(string.Empty)))
                {
                    var defaultMessage = "The property '{0}' is mandatory.";
                    var displayName = validationContext.DisplayName;

                    if (validationContext.GetService(typeof(ApplicationService)) is ApplicationService appService)
                    {
                        defaultMessage = appService.GetStaticText("ValidationMessageDefault_RuleRequired");
                        var displayText = appService.GetPropertyDisplayName(validationContext.ObjectType.GetCorrectType().FullName, validationContext.MemberName);
                        if (!string.IsNullOrWhiteSpace(displayText))
                            displayName = displayText;
                    }

                    return new ValidationResult(
                        !string.IsNullOrWhiteSpace(ErrorMessage) ? ErrorMessage : string.Format(defaultMessage, displayName),
                        string.IsNullOrWhiteSpace(ElementId) ? new[] { validationContext.MemberName }  : ElementId.Split(","));
                }
            }

            return ValidationResult.Success;
        }

    }

}