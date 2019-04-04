namespace Zambon.Core.Database.Domain.Interfaces
{
    /// <summary>
    /// Represents an entity type in database model, will automatically map into a table with the same name from OnConfiguring.
    /// </summary>
    public interface IEntity
    {

        ///// <summary>
        ///// Called when executing OnConfiguring from CoreContext.
        ///// </summary>
        ///// <param name="args">Arguments.</param>
        //void Configure(object[] args);

    }
}