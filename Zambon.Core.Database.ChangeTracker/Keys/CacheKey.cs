using Zambon.Core.Database.ChangeTracker.Services;
using System;

namespace Zambon.Core.Database.ChangeTracker
{
    /// <summary>
    /// Represent the user key used to access the stored objects in database change tracker cache.
    /// </summary>
    public class CacheKey
    {
        /// <summary>
        /// Guid unique to each user.
        /// </summary>
        public Guid DatabaseKey { get; private set; }

        /// <summary>
        /// Current user ID.
        /// </summary>
        public int? CurrentUserId { get; private set; }

        /// <summary>
        /// The current form Key.
        /// </summary>
        public Guid? FormKey { get; set; }


        public CacheKey(ICacheKeyService cacheKeyService)
        {
            DatabaseKey = cacheKeyService.RetrieveDatabaseKey();
            CurrentUserId = cacheKeyService.RetrieveUserId();
            FormKey = cacheKeyService.RetrieveFormKey();
        }



        /// <summary>
        /// Returns the hash code for this string.
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
        {
            var compare = (CacheKey)obj;
            return
                DatabaseKey.Equals(compare.DatabaseKey)
                && ((CurrentUserId == null && compare.DatabaseKey == null) || (CurrentUserId?.Equals(compare.CurrentUserId) ?? false))
                && ((FormKey == null && compare.FormKey == null) || (FormKey?.Equals(compare.FormKey) ?? false));
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>The value of this System.Guid plus the current user ID, formatted by using the format specifier as follows: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx_####.</returns>
        public override string ToString()
            => DatabaseKey.ToString()
            + (CurrentUserId != null ? $"_{CurrentUserId.ToString()}" : "")
            + (FormKey != null ? $"_{FormKey.ToString()}" : "");
    }
}