using System;
using Zambon.Core.Database.ChangeTracker.Keys;

namespace Zambon.Core.Database.ChangeTracker.Services
{
    /// <summary>
    /// Service used to store and retrieve the database key under current session.
    /// </summary>
    public interface IUserKeyProvider
    {
        /// <summary>
        /// Stores the key in user current session.
        /// </summary>
        /// <param name="databaseKey">Unique key to the user.</param>
        void StoreKey(Guid databaseKey);

        /// <summary>
        /// Retrieves the current change tracker key.
        /// </summary>
        /// <returns>Returns an object of type InstanceKey.</returns>
        UserKey RetrieveKey();
    }
}
