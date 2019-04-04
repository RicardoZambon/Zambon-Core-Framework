using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Zambon.Core.Database.ChangeTracker.Extensions;
using System;

namespace Zambon.Core.Database.ChangeTracker
{
    /// <summary>
    /// Represents the key for each object save in custom ChangeTracker.
    /// </summary>
    [Serializable]
    public class StoreKey
    {
        /// <summary>
        /// The ModelType of the entity as string.
        /// </summary>
        public string ModelType { get; private set; }

        /// <summary>
        /// The ID of the entity as integer.
        /// </summary>
        public object[] EntityKeys { get; private set; }


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="modelType">The ModelType of the entity as string.</param>
        /// <param name="entityKeys">The keys of the entity.</param>
        public StoreKey(string modelType, params object[] entityKeys)
        {
            ModelType = modelType;
            EntityKeys = entityKeys;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="entry">The object instance.</param>
        public StoreKey(EntityEntry entry)
        {
            ModelType = ProxyUtil.GetUnproxiedType(entry.Entity).Name;
            EntityKeys = entry.GetKeyValues();
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
            var compare = (StoreKey)obj;
            return ModelType.Equals(compare.ModelType) && EntityKeys.Equals(compare.EntityKeys);
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>The value of this System.Guid plus the current user ID, formatted by using the format specifier as follows: xxxx_####.</returns>
        public override string ToString()
        {
            return $"{ModelType}_{string.Join("_", EntityKeys)}";
        }
    }
}