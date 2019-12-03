using Zambon.Core.Database.Domain.Interfaces;

namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Represents an entity type in database model, will automatically map into a table with the class name from OnConfiguring.
    /// </summary>
    public interface IEntity : IBaseObject
    {
    }
}