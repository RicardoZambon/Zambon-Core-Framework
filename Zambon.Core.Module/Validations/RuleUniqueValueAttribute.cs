using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using Zambon.Core.Database;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.Validations
{
    /// <summary>
    /// Specifies that a data field value can not be duplicated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RuleUniqueValueAttribute : ValidationAttribute
    {

        /// <summary>
        /// The condition when the validation should occurs.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// If should ignore when the field is empty.
        /// </summary>
        public bool IgnoreEmptyStrings { get; set; } = true;


        /// <summary>
        /// Initializes a new instance of the Zambon.Core.Module.Validations.RuleUniqueValueAttribute.
        /// </summary>
        public RuleUniqueValueAttribute()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the Zambon.Core.Module.Validations.RuleUniqueValueAttribute.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        public RuleUniqueValueAttribute(string errorMessage) : base(errorMessage)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Zambon.Core.Module.Validations.RuleUniqueValueAttribute.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        /// <param name="condition">The condition when the validation should occurs.</param>
        public RuleUniqueValueAttribute(string errorMessage, string condition) : base(errorMessage)
        {
            Condition = condition;
        }

        /// <summary>
        /// Initializes a new instance of the Zambon.Core.Module.Validations.RuleUniqueValueAttribute.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        /// <param name="condition">The condition when the validation should occurs.</param>
        /// <param name="ignoreEmptyStrings">If should ignore when the field is empty.</param>
        public RuleUniqueValueAttribute(string errorMessage, string condition, bool ignoreEmptyStrings) : base(errorMessage)
        {
            Condition = condition;
            IgnoreEmptyStrings = ignoreEmptyStrings;
        }


        /// <summary>
        /// Checks that the value of the required data field is not duplicated in database.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var expressionService = validationContext.GetService(typeof(ExpressionsService)) as ExpressionsService;
            if ((string.IsNullOrWhiteSpace(Condition)
                || expressionService == null || expressionService.IsConditionApplicable(Condition, validationContext.ObjectInstance))
                && value != null
                && (value is string stringValue && (!string.IsNullOrWhiteSpace(stringValue) || !IgnoreEmptyStrings)))
            {
                if (validationContext.GetService(typeof(CoreDbContext)) is CoreDbContext ctx && validationContext.ObjectInstance is IKeyed dbInstance)
                {
                    string query = validationContext.ObjectInstance is BaseDBObject ? "IsDeleted = false AND " : "";
                    switch (value)
                    {
                        case string valueString:
                            query += "ID != @0 AND {0}.ToLower().Equals(@1.ToLower())";
                            break;
                        default:
                            query = "AND ID != @0 AND {0} = @1";
                            break;
                    }

                    if (ctx.Set(validationContext.ObjectType.GetUnproxiedType()).Count(string.Format(query, validationContext.MemberName), dbInstance.ID, value) > 0)
                    {
                        var defaultMessage = "The property '{0}' value is already registered.";
                        var displayName = validationContext.DisplayName;

                        if (validationContext.GetService(typeof(ApplicationService)) is ApplicationService app)
                        {
                            defaultMessage = app.GetStaticText("ValidationMessageDefault_RuleUniqueValue");
                            var displayText = app.GetPropertyDisplayName(validationContext.ObjectType.GetUnproxiedType().FullName, validationContext.MemberName);
                            if (!string.IsNullOrWhiteSpace(displayText))
                                displayName = displayText;
                        }

                        return new ValidationResult(
                            !string.IsNullOrWhiteSpace(ErrorMessage) ? ErrorMessage : string.Format(defaultMessage, displayName),
                            new[] { validationContext.MemberName });
                    }
                }
            }
            return ValidationResult.Success;
        }

    }

}