using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zambon.Core.Database.ChangeTracker.Extensions.Internal
{
    internal static class DbContextExtension
    {
        private static MethodInfo _loadPropertiesMethod;
        private static MethodInfo LoadPropertiesMethod
        {
            get
            {
                if (_loadPropertiesMethod == null)
                {
                    _loadPropertiesMethod = typeof(DbContextExtension).GetMethods().FirstOrDefault(x => x.Name == nameof(LoadProperties) && x.IsGenericMethod);
                }
                return _loadPropertiesMethod;
            }
        }

        private static MethodInfo _markAsDeletedMethod;
        private static MethodInfo MarkAsDeletedMethod
        {
            get
            {
                if (_markAsDeletedMethod == null)
                {
                    _markAsDeletedMethod = typeof(DbContextExtension).GetMethods().FirstOrDefault(x => x.Name == nameof(MarkAsDeletedMethod) && x.IsGenericMethod);
                }
                return _markAsDeletedMethod;
            }
        }


        /// <summary>
        /// Load the properties dictionary merging with the database values.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="modelType">The model type of the database entity.</param>
        /// <param name="keys">Array with the key properties values.</param>
        /// <param name="propertyValues">A dictionary with the changed properties and their respective values.</param>
        /// <returns>Return the database object instance with the updated values.</returns>
        public static object LoadProperties(this DbContext dbContext, string modelType, object[] keys, Dictionary<string, object> propertyValues)
        {
            var entityType = dbContext.Model.FindEntityType(modelType);
            return LoadPropertiesMethod.MakeGenericMethod(entityType.ClrType).Invoke(null, new object[] { dbContext, keys, propertyValues });
        }

        /// <summary>
        /// Load the properties dictionary merging with the database values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the database entity.</typeparam>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="keys">Array with the key properties values.</param>
        /// <param name="propertyValues">A dictionary with the changed properties and their respective values.</param>
        /// <returns>Return the database object instance with the updated values.</returns>
        public static TEntity LoadProperties<TEntity>(this DbContext dbContext, object[] keys, Dictionary<string, object> propertyValues) where TEntity : class
        {
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));

            EntityEntry<TEntity> entry;
            var entity = dbContext.Find<TEntity>(keys);
            if (entity == null || dbContext.IsNewEntry(entity))
            {
                if (entity == null)
                {
                    entity = dbContext.CreateProxy<TEntity>();
                    var key = entityType.FindPrimaryKey();
                    for (var i = 0; i < key.Properties.Count; i++)
                    {
                        typeof(TEntity).GetProperty(key.Properties[i].Name).SetValue(entity, keys[i]);
                    }
                }
                entry = dbContext.Add(entity);
                entry.State = EntityState.Added;
            }
            else
            {
                entry = dbContext.Attach(entity);
                entry.State = EntityState.Modified;
            }
            entry.CurrentValues.SetValues(propertyValues);
            return entry.Entity;
        }


        public static object MarkAsDeleted(this DbContext dbContext, string modelType, object[] keys)
        {
            var entityType = dbContext.Model.FindEntityType(modelType);
            return MarkAsDeletedMethod.MakeGenericMethod(entityType.ClrType).Invoke(null, new object[] { dbContext, keys });
        }

        public static TEntity MarkAsDeleted<TEntity>(this DbContext dbContext, object[] keys) where TEntity : class
        {
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
            var entity = dbContext.Find<TEntity>(keys);

            var entry = dbContext.Entry(entity);
            entry.State = EntityState.Deleted;

            return entry.Entity;
        }
    }
}