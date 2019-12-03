using Microsoft.EntityFrameworkCore;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.Domain.Interfaces;

namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Represents base database classes.
    /// </summary>
    public interface IDbObject : IEntity, IKeyed, ITrackableEntity, ICustomValidatableObject
    {
        /// <summary>
        /// Executed whether the object is being deleted.
        /// </summary>
        /// <param name="dbContext">The current database context instance.</param>
        void OnDeleting(DbContext dbContext);

        /// <summary>
        /// Executed whether the object is being saved into database.
        /// </summary>
        /// <param name="dbContext">The current database context instance.</param>
        void OnSaving(DbContext dbContext);
    }
}