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
                var keyProperties = entry.GetKeyNames();
                return entry.Properties.Where(x => !keyProperties.Contains(x.Metadata.Name)).ToDictionary(k => k.Metadata.Name, v => v.CurrentValue);
            }

            var dbData = entry.GetDatabaseValues();
            return entry.Properties.Where(
                x => x.Metadata.Name != "Discriminator"
                    && ((entry.CurrentValues[x.Metadata.Name] == null && dbData[x.Metadata.Name] != null)
                    || (entry.CurrentValues[x.Metadata.Name] != null && !entry.CurrentValues[x.Metadata.Name].Equals(dbData[x.Metadata.Name]))
                )).ToDictionary(k => k.Metadata.Name, v => entry.CurrentValues[v.Metadata.Name]);
        }


        /// <summary>
        /// Returns the entity type for the entity entry.
        /// </summary>
        /// <param name="entry">The database entity entry instance from the context.</param>
        /// <returns>Returns database entity type.</returns>
        public static IEntityType GetEntityType(this EntityEntry entry)
            => entry.Context.GetEntityType(entry.Entity.GetType());

        /// <summary>
        /// Get the primary key names from the entry instance.
        /// </summary>
        /// <param name="entry">The database entry instance from DB context.</param>
        /// <returns>Returns an collection of all key names.</returns>
        public static IReadOnlyList<string> GetKeyNames(this EntityEntry entry)
            => entry.Context.GetKeyNames(entry.Entity.GetType());

        /// <summary>
        /// Get the primary key current values from the entry instance.
        /// </summary>
        /// <param name="entry">The database entry instance from DB context.</param>
        /// <returns>Returns an collection of all key values.</returns>
        public static IReadOnlyList<object> GetKeyValues(this EntityEntry entry)
            => entry.Context.GetKeyProperties(entry.Entity.GetType()).Select(k => entry.CurrentValues[k.Name]).ToArray();
    }
}