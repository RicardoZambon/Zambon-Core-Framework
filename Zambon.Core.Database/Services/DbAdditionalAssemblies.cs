using System.Collections.Generic;

namespace Zambon.Core.Database.Services
{
    /// <summary>
    /// Defines additional assemblies to use when configuring the database.
    /// </summary>
    public interface IDbAdditionalAssemblies
    {
        /// <summary>
        /// List of assemblies names.
        /// </summary>
        IEnumerable<string> ReferencedAssemblies { get; }
    }
}