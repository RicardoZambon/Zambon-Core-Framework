using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using Zambon.Core.Database.ChangeTracker.Extensions;

namespace Zambon.Core.Database.ChangeTracker
{
    /// <summary>
    /// Represents the key for each object save in custom ChangeTracker.
    /// </summary>
    [Serializable]
    public class StoredInstanceKey
    {
        /// <summary>
        /// The root model type of the entity as string.
        /// </summary>
        public string ModelType { get; private set; }

        /// <summary>
        /// The actual entity type as string.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// The ID of the entity as integer.
        /// </summary>
        public object[] EntityKeys { get; private set; }

        public ChangeTrackerActions Action { get; private set; }


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="entityKeys">The keys of the entity.</param>
        public StoredInstanceKey(DbContext dbContext, Type entityType, params object[] entityKeys)
        {
            var modelEntityType = dbContext.Model.FindEntityType(entityType.GetUnproxiedType());
            ModelType = modelEntityType.RootType().Name;
            EntityType = modelEntityType.Name;
            EntityKeys = entityKeys;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="entry">The object instance.</param>
        public StoredInstanceKey(EntityEntry entry, ChangeTrackerActions action)
        {
            ModelType = entry.Metadata.Model.FindEntityType(entry.Entity.GetUnproxiedType()).RootType().Name;
            EntityType = entry.Metadata.Model.FindEntityType(entry.Entity.GetUnproxiedType()).Name;
            EntityKeys = entry.GetKeyValues();
            Action = ChangeTrackerActions.Delete;
        }


        public string GetFullKey(CacheKey cacheKey)
        {
            return cacheKey.ToString() + "_" + this.ToString();
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
            var compare = (StoredInstanceKey)obj;
            for (var k = 0; k < EntityKeys.Length; k++)
            {
                if (!EntityKeys[k].Equals(compare.EntityKeys[k]))
                {
                    return false;
                }
            }
            return ModelType.Equals(compare.ModelType);
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>The value of this System.Guid plus the current user ID, formatted by using the format specifier as follows: xxxx_####.</returns>
        public override string ToString()
            => $"{ModelType}_{string.Join("_", EntityKeys)}";
    }
}