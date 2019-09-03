using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zambon.Core.Database.Domain.Interfaces
{
    /// <summary>
    /// Represents database classes with custom validation method.
    /// </summary>
    public interface ICustomValidatableObject
    {
        /// <summary>
        /// Called when validating the object using the custom validation methods.
        /// </summary>
        /// <param name="validationContext">The validation context instance.</param>
        /// <returns>Returns a list of invalid properties names and their respective errors descriptions.</returns>
        IDictionary<object, object> ValidateData(ValidationContext validationContext);
    }
}