using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Defines configuration methods to use inside the database objects.
    /// </summary>
    public interface IConfigurableQuery
    {
        /// <summary>
        /// Called when executing OnConfiguring from CoreContext.
        /// </summary>
        /// <param name="queryBuilder">The object that can be used to configure a given query type in the model.</param>
        void Configure(QueryTypeBuilder queryBuilder);
    }
}