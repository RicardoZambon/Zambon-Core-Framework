using System;

namespace Zambon.Core.Database.ChangeTracker
{
    /// <summary>
    /// Represent the user key used to access the stored objects in database change tracker cache.
    /// </summary>
    public class InstanceKey
    {

        /// <summary>
        /// Guid unique to each user.
        /// </summary>
        public Guid DatabaseKey { get; private set; }

        /// <summary>
        /// Current user ID.
        /// </summary>
        public int CurrentUserId { get; private set; }


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="databaseKey">Guid unique to each user.</param>
        /// <param name="currentUserId">Current user ID.</param>
        public InstanceKey(Guid databaseKey, int currentUserId)
        {
            DatabaseKey = databaseKey;
            CurrentUserId = currentUserId;
        }


        /// <summary>
        /// Returns the hash code for this string.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified System.Guid object represent the same value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if g is equal to this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            var compare = (InstanceKey)obj;
            return DatabaseKey.Equals(compare.DatabaseKey) && CurrentUserId.Equals(compare.CurrentUserId);
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>The value of this System.Guid plus the current user ID, formatted by using the format specifier as follows: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx_####.</returns>
        public override string ToString()
        {
            return $"{DatabaseKey.ToString()}_{CurrentUserId.ToString()}";
        }

    }
}