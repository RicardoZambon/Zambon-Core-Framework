namespace Zambon.Core.Database.Interfaces
{
    /// <summary>
    /// Represents base database classes.
    /// </summary>
    public interface IDBObject : IEntity, IKeyed, ICustomValidated
    {
        /// <summary>
        /// Executed whether the object is being deleted.
        /// </summary>
        /// <param name="ctx">The current database context instance.</param>
        void OnDeleting(CoreDbContext ctx);

        /// <summary>
        /// Executed whether the object is being saved into database.
        /// </summary>
        /// <param name="ctx">The current database context instance.</param>
        void OnSaving(CoreDbContext ctx);

    }
}