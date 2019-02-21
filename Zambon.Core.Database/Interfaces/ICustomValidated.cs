using System.Collections.Generic;

namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Represents database classes with custom validation method.
    /// </summary>
    public interface ICustomValidated
    {
        /// <summary>
        /// Called when validating the object using the custom validation methods.
        /// </summary>
        /// <param name="ctx">The current database context instance.</param>
        /// <returns>Retuns a list of invalid properties names and their respective errors descriptions.</returns>
        List<KeyValuePair<string, string>> ValidateData(CoreDbContext ctx);
    }
}
