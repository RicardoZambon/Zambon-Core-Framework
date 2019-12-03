using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Zambon.Core.Database.ChangeTracker.Extensions
{
    /// <summary>
    /// Extension methods used by DbContext.
    /// </summary>
    public static class DbContextExtension
    {
        /// <summary>
        /// Search for the model entity type of the entity instance.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entity">The object instance to get the entity type.</param>
        /// <returns>Returns the entity type, null if no entity type was found.</returns>
        public static IEntityType GetEntityType(this DbContext dbContext, object entity)
            => dbContext.Model.FindEntityType(entity.GetType());

        /// <summary>
        /// Search for the model entity type of the type.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entityType">The actual type to get the entity type.</param>
        /// <returns>Returns the entity type, null if no entity type was found.</returns>
        public static IEntityType GetEntityType(this DbContext dbContext, Type entityType)
            => dbContext.Model.FindEntityType(entityType);


        /// <summary>
        /// Returns all the keys properties for the entity.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>Returns all the keys properties for the entity.</returns>
        public static IReadOnlyList<IProperty> GetKeyProperties(this DbContext dbContext, Type entityType)
            => dbContext.GetEntityType(entityType).FindPrimaryKey()?.Properties;

        /// <summary>
        /// Returns all the keys property names for the entity.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>Returns all the keys property names for the entity.</returns>
        public static IReadOnlyList<string> GetKeyNames(this DbContext dbContext, Type entityType)
            => dbContext.GetKeyProperties(entityType).Select(x => x.Name).ToArray();

        /// <summary>
        /// Returns all the key values for the entity.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entity">The entity instance to get the key values.</param>
        /// <returns>Returns all the key values for the entity.</returns>
        public static IReadOnlyList<object> GetKeyValues(this DbContext dbContext, object entity)
            => dbContext.Entry(entity).GetKeyValues();


        /// <summary>
        /// Checks if the entry already exists in database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entity">The object instance to check.</param>
        /// <returns>Returns true if the entry does not exists yet in database.</returns>
        public static bool IsNewEntry<T>(this DbContext dbContext, T entity) where T : class
        {
            if (dbContext.GetEntityType(typeof(T)) == null)
            {
                throw new Exception("Missing entity type");
            }
            return dbContext.Set<T>().AsNoTracking().FirstOrDefault(BuildLambda<T>(dbContext.GetKeyProperties(typeof(T)), dbContext.GetKeyValues(entity))) == null;
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


        private static Expression<Func<TEntity, bool>> BuildLambda<TEntity>(IReadOnlyList<IProperty> keyProperties, IReadOnlyList<object> keyValues) where TEntity : class
        {
            var entityParameter = Expression.Parameter(typeof(TEntity), "e");
            return Expression.Lambda<Func<TEntity, bool>>(BuildPredicate(keyProperties, keyValues, entityParameter), entityParameter);
        }

        private static BinaryExpression BuildPredicate(IReadOnlyList<IProperty> keyProperties, IReadOnlyList<object> keyValues, ParameterExpression entityParameter)
        {
            var predicate = GenerateEqualExpression(keyProperties[0], 0);
            for (var i = 1; i < keyProperties.Count; i++)
            {
                predicate = Expression.AndAlso(predicate, GenerateEqualExpression(keyProperties[i], i));
            }
            return predicate;

            BinaryExpression GenerateEqualExpression(IProperty property, int i) => Expression.Equal(Expression.Property(entityParameter, property.Name), Expression.Constant(keyValues[i]));
        }
    }
}