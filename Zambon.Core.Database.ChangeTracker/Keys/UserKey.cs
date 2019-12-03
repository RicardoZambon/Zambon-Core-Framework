using System;

namespace Zambon.Core.Database.ChangeTracker.Keys
{
    /// <summary>
    /// The change tracker key used to access stored objects in database cache.
    /// </summary>
    public class UserKey
    {
        /// <summary>
        /// The user access key, should be unique to each user.
        /// </summary>
        public Guid DatabaseKey { get; private set; }

        /// <summary>
        /// Current user ID.
        /// </summary>
        public int CurrentUserId { get; private set; }


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="databaseKey">The user access key, should be unique to each user.</param>
        /// <param name="currentUserId">Current user ID.</param>
        public UserKey(Guid databaseKey, int currentUserId)
        {
            DatabaseKey = databaseKey;
            CurrentUserId = currentUserId;
        }


        /// <summary>
        /// Returns the hash code for this change tracker key.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();

        /// <summary>
        /// Returns a value indicating whether this instance and a specified System.Guid object represent the same value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if g is equal to this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
            => DatabaseKey.Equals(((UserKey)obj).DatabaseKey) && CurrentUserId.Equals(((UserKey)obj).CurrentUserId);

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>The value of this System.Guid plus the current user ID, formatted by using the format specifier as follows: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx_####.</returns>
        public override string ToString()
            => $"{DatabaseKey.ToString()}|{CurrentUserId.ToString()}";
    }
}