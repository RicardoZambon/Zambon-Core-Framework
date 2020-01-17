using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Defines configuration methods to use inside the IEntity object.
    /// </summary>
    public interface IConfigurableEntity
    {
        void OnConfiguringEntity(EntityTypeBuilder entityBuilder);
    }
}