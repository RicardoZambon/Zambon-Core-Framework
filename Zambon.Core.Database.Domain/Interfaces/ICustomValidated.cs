using System.Collections.Generic;

namespace Zambon.Core.Database.Domain.Interfaces
{
    /// <summary>
    /// Represents database classes with custom validation method.
    /// </summary>
    public interface ICustomValidated
    {
        /// <summary>
        /// Called when validating the object using the custom validation methods.
        /// </summary>
        /// <param name="args">Arguments list.</param>
        /// <returns>Returns a list of invalid properties names and their respective errors descriptions.</returns>
        List<KeyValuePair<string, string>> ValidateData(object[] args);
    }
}
