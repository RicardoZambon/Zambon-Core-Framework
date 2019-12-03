using Microsoft.EntityFrameworkCore;

namespace Zambon.Core.Database.Services
{
    /// <summary>
    /// Defines additional assemblies to use when configuring the database.
    /// </summary>
    public class DbAdditionalAssemblies<TDbContext> : IDbAdditionalAssemblies where TDbContext : DbContext
    {
        /// <summary>
        /// List of assemblies names.
        /// </summary>
        public string[] ReferencedAssemblies { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="referencedAssemblies">List of assemblies names.</param>
        public DbAdditionalAssemblies(string[] referencedAssemblies)
        {
            ReferencedAssemblies = referencedAssemblies;
        }
    }
}