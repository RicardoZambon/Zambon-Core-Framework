namespace Zambon.Core.Database.Domain.Interfaces
{
    /// <summary>
    /// Represents database classes with a int primary key.
    /// </summary>
    public interface IKeyed
    {
        /// <summary>
        /// Primary key of the database entity.
        /// </summary>
        int Id { get; set; }
    }
}