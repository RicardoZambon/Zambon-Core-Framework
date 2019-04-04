namespace Zambon.Core.Database.Domain.Interfaces
{
    /// <summary>
    /// Represents base database classes.
    /// </summary>
    public interface IDBObject : IEntity, IKeyed, ICustomValidated
    {
        /// <summary>
        /// Executed whether the object is being deleted.
        /// </summary>
        /// <param name="args">Arguments list.</param>
        void OnDeleting(object[] args);

        /// <summary>
        /// Executed whether the object is being saved into database.
        /// </summary>
        /// <param name="args">Arguments list.</param>
        void OnSaving(object[] args);

    }
}