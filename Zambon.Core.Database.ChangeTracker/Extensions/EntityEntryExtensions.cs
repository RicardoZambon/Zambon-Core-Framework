using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zambon.Core.Database.ChangeTracker.Extensions
{
    /// <summary>
    /// Extension methods used in entity entries.
    /// </summary>
    public static class EntityEntryExtension
    {
        internal static Dictionary<string, object> GetChangedProperties<T>(this EntityEntry<T> entry) where T : class
        {
            if (entry.Context.IsNewEntry(entry.Entity))
            {
                var keyProperties = entry.GetKeys();
                return entry.Properties.Where(x => !keyProperties.Contains(x.Metadata)).ToDictionary(k => k.Metadata.Name, v => v.CurrentValue);
            }

            var dbData = entry.GetDatabaseValues();
            return entry.Properties.Where(
                x => x.Metadata.Name != "Discriminator" //TODO: Maybe check for ShadowProperties?
                    && ((entry.CurrentValues[x.Metadata.Name] == null && dbData[x.Metadata.Name] != null)
                    || (entry.CurrentValues[x.Metadata.Name] != null && !entry.CurrentValues[x.Metadata.Name].Equals(dbData[x.Metadata.Name]))
                )).ToDictionary(k => k.Metadata.Name, v => entry.CurrentValues[v.Metadata.Name]);
        }

        /// <summary>
        /// Returns all the keys properties for the entity.
        /// </summary>
        /// <param name="entry">The database entity entry instance from context.</param>
        /// <returns>Returns all the keys properties for the entity.</returns>
        public static IReadOnlyList<IProperty> GetKeys(this EntityEntry entry)
        {
            var entityType = entry.Context.Model.FindEntityType(entry.Entity.GetUnproxiedType());
            return entityType.FindPrimaryKey().Properties.ToArray();
        }

        /// <summary>
        /// Get an array of the object key values.
        /// </summary>
        /// <param name="entry">The database entity entry instance from context.</param>
        /// <returns>Returns an array of the object key values.</returns>
        public static object[] GetKeyValues(this EntityEntry entry)
        {
            var keyProperties = GetKeys(entry);
            var keys = new object[keyProperties.Count];
            for (var i = 0; i < keys.Length; i++)
            {
                keys[i] = entry.CurrentValues[keyProperties[i]];
            }
            return keys;
        }
    }
}