using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Defines configuration methods to use inside the IEntity object.
    /// </summary>
    public interface IConfigurableEntity
    {
        /// <summary>
        /// Called when executing OnConfiguring from CoreContext.
        /// </summary>
        /// <param name="entityBuilder">The object that can be used to configure a given entity type in the model.</param>
        void ConfigureEntity(EntityTypeBuilder entityBuilder);
    }
}