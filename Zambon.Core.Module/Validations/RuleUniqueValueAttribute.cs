using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RuleUniqueValueAttribute : ValidationAttribute
    {

        public string Condition { get; set; }

        public bool IgnoreEmptyStrings { get; set; } = true;


        public RuleUniqueValueAttribute()
        {
            
        }

        public RuleUniqueValueAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public RuleUniqueValueAttribute(string errorMessage, string condition) : base(errorMessage)
        {
            Condition = condition;
        }

        public RuleUniqueValueAttribute(string errorMessage, string condition, bool ignoreEmptyStrings) : base(errorMessage)
        {
            Condition = condition;
            IgnoreEmptyStrings = ignoreEmptyStrings;
        }


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

                    if (ctx.Set(validationContext.ObjectType.GetCorrectType()).Count(string.Format(query, validationContext.MemberName), dbInstance.ID, value) > 0)
                    {
                        var defaultMessage = "The property '{0}' value is already registered.";
                        var displayName = validationContext.DisplayName;

                        if (validationContext.GetService(typeof(ApplicationService)) is ApplicationService app)
                        {
                            defaultMessage = app.GetStaticText("ValidationMessageDefault_RuleUniqueValue");
                            var displayText = app.GetPropertyDisplayName(validationContext.ObjectType.GetCorrectType().FullName, validationContext.MemberName);
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