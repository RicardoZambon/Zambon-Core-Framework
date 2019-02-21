using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Represents an entity type in database model, will automatically map into a table with the same name from OnConfiguring.
    /// </summary>
    public interface IEntity
    {

        /// <summary>
        /// Called when executing OnConfiguring from CoreContext.
        /// </summary>
        /// <param name="entity">The object that can be used to configure a given entity type in the model.</param>
        void ConfigureEntity(EntityTypeBuilder entity);

    }
}