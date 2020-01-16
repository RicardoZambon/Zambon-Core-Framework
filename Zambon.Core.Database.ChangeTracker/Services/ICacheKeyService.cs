using System;

namespace Zambon.Core.Database.ChangeTracker.Services
{
    /// <summary>
    /// Service used to store and retrieve the instance key in user's session.
    /// </summary>
    public interface ICacheKeyService
    {
        /// <summary>
        /// Retrieves the current user instance key.
        /// </summary>
        /// <returns>Returns an object of type InstanceKey.</returns>
        Guid RetrieveDatabaseKey();

        int RetrieveUserId();

        /// <summary>
        /// Retrieves the unique identifier for the current form. 
        /// </summary>
        /// <returns>Returns the Guid object, null if not found the form key.</returns>
        Guid RetrieveFormKey();
    }
}