using System;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.Validations
{
    /// <summary>
    /// Specifies that a data field value is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RuleRequiredAttribute : ValidationAttribute
    {

        /// <summary>
        /// The condition when the validation should occurs.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// The ID of the element in the page separated by ",", if null will use the data field name.
        /// </summary>
        public string ElementId { get; set; }


        /// <summary>
        /// Initializes a new instance of the Zambon.Core.Module.Validations.RuleRequiredAttribute.
        /// </summary>
        public RuleRequiredAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Zambon.Core.Module.Validations.RuleRequiredAttribute.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        public RuleRequiredAttribute(string errorMessage) : base(errorMessage)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Zambon.Core.Module.Validations.RuleRequiredAttribute.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        /// <param name="Condition">The condition when the validation should occurs.</param>
        public RuleRequiredAttribute(string errorMessage, string Condition) : base(errorMessage)
        {
            this.Condition = Condition;
        }


        /// <summary>
        /// Checks that the value of the required data field is not empty.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
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
                        var displayText = appService.GetPropertyDisplayName(validationContext.ObjectType.GetUnproxiedType().FullName, validationContext.MemberName);
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