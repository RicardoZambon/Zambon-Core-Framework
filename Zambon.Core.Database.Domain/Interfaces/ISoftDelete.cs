namespace Zambon.Core.Database.Domain.Interfaces
{
    /// <summary>
    /// Interface for objects that should have a property to set when deleting and not actually deleted from database.
    /// </summary>
    public interface ISoftDelete : IBaseObject
    {
        /// <summary>
        /// Determines when the object is deleted and should be ignored from the application queries.
        /// </summary>
        bool IsDeleted { get; set; }
    }
}