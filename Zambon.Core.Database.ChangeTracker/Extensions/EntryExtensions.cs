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
    public static class EntityEntryExtensions
    {
        internal static Dictionary<string, object> GetChangedProperties<T>(this EntityEntry<T> entry) where T : class
        {
            if (entry.Context.IsNewEntry(entry.Entity))
            {
                var keyProperties = GetKeys(entry);
                return entry.Properties.Where(x => !keyProperties.Contains(x.Metadata)).ToDictionary(k => k.Metadata.Name, v => v.CurrentValue);
            }

            var dbData = entry.GetDatabaseValues();
            return entry.Properties.Where(
                x => x.Metadata.Name != "Discriminator" //TODO: Maybe check for ShadowProperties?
                    && ((entry.CurrentValues[x.Metadata.Name] == null && dbData[x.Metadata.Name] != null)
                    || (entry.CurrentValues[x.Metadata.Name] != null && !entry.CurrentValues[x.Metadata.Name].Equals(dbData[x.Metadata.Name]))
                )).ToDictionary(k => k.Metadata.Name, v => entry.CurrentValues[v.Metadata.Name]);
        }

        internal static IReadOnlyList<IProperty> GetKeys(this EntityEntry entry)
        {
            var entityType = entry.Context.Model.FindEntityType(entry.Entity.GetUnproxiedType());
            return entityType.FindPrimaryKey().Properties.ToArray();
        }

        internal static object[] GetKeyValues(this EntityEntry entry)
        {
            var keyProperties = GetKeys(entry);
            var keys = new object[keyProperties.Count];
            for (var i = 0; i < keys.Length; i++)
                keys[i] = entry.CurrentValues[keyProperties[i]];
            return keys;
        }        
    }
}