using Microsoft.EntityFrameworkCore;

namespace Zambon.Core.Database
{
    /// <summary>
    /// Generates initial database data
    /// </summary>
    public interface ICoreDbSeedData
    {
        /// <summary>
        /// Used to seed the initial data, called after adding the entity types into the model.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically define extension methods on this object that allow you to configure aspects of the model that are specific to a given database.</param>
        void SeedData(ModelBuilder modelBuilder);
    }
}