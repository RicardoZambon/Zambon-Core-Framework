using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using Zambon.Core.Database.ChangeTracker.Extensions;

namespace Zambon.Core.Database.ChangeTracker.Keys
{
    /// <summary>
    /// Represents the key to track each object instance saved in change tracker.
    /// </summary>
    [Serializable]
    public class EntityInstanceKey
    {
        #region Properties

        /// <summary>
        /// The name of the entity type stored.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// The database keys to unique identify the current object.
        /// </summary>
        public object[] EntityKeys { get; private set; }

        #endregion

        #region Constructors

        ///// <summary>
        ///// Default constructor.
        ///// </summary>
        ///// <param name="formKey">The parent form key, used to separate same user and different opened pages/forms.</param>
        ///// <param name="dbContext">The database context instance.</param>
        ///// <param name="type">The type of the entity.</param>
        ///// <param name="entityKeys">The keys of the entity.</param>
        //public EntityInstanceKey(Guid formKey, DbContext dbContext, Type type, params object[] entityKeys)
        //{
        //    FormKey = formKey;
        //    var entityType = dbContext.GetEntityType(type);
        //    EntityName = entityType.Name;
        //    EntityKeys = entityKeys;
        //}

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="entry">The object instance.</param>
        public EntityInstanceKey(EntityEntry entry)
        {
            EntityName = entry.GetEntityType().Name;
            EntityKeys = entry.GetKeyNames().ToArray();
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="keys">Array of key values.</param>
        public EntityInstanceKey(DbContext dbContext, Type entityType, object[] keys)
        {
            EntityName = dbContext.GetEntityType(entityType).Name;
            EntityKeys = keys;
        }

        #endregion

        #region Overrides

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
            var compare = (EntityInstanceKey)obj;
            for (var k = 0; k < EntityKeys.Length; k++)
            {
                if (!EntityKeys[k].Equals(compare.EntityKeys[k]))
                {
                    return false;
                }
            }
            return EntityName.Equals(compare.EntityName);
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>The value of this System.Guid plus the current user ID, formatted by using the format specifier as follows: xxxx_####.</returns>
        public override string ToString()
            => $"{EntityName}#{string.Join("|", EntityKeys)}";

        #endregion
    }
}