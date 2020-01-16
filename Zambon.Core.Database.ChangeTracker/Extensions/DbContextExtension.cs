﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Zambon.Core.Database.ChangeTracker.Extensions
{
    /// <summary>
    /// Extension methods used by DbContext.
    /// </summary>
    public static class DbContextExtension
    {
        private static MethodInfo _isNewEntryMethod;
        private static MethodInfo IsNewEntryMethod
        {
            get
            {
                if (_isNewEntryMethod == null)
                {
                    _isNewEntryMethod = typeof(DbContextExtension).GetMethods().FirstOrDefault(x => x.Name == nameof(IsNewEntry) && x.IsGenericMethod);
                }
                return _isNewEntryMethod;
            }
        }

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


        /// <summary>
        /// Returns all the keys properties for the entity.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entityType">The entity type mapped in database.</param>
        /// <returns>Returns all the keys properties for the entity.</returns>
        public static string[] GetKeyProperties(this DbContext dbContext, Type entityType)
            => dbContext.Model.FindEntityType(entityType?.GetUnproxiedType()).FindPrimaryKey()?.Properties?.Select(x => x.Name)?.ToArray() ?? new string[0];

        /// <summary>
        /// Returns all the keys properties for the entity.
        /// </summary>
        /// <typeparam name="T">The entity type mapped in database.</typeparam>
        /// <param name="dbContext">The database context instance.</param>
        /// <returns>Returns all the keys properties for the entity.</returns>
        public static IReadOnlyList<IProperty> GetKeys<T>(this DbContext dbContext) where T : class
            => dbContext.Model.FindEntityType(typeof(T).GetUnproxiedType()).FindPrimaryKey()?.Properties;


        /// <summary>
        /// Get an array of the object key values.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="erntity">The database object instance.</param>
        /// <returns>Returns an array of the object key values.</returns>
        public static object[] GetKeyValues(this DbContext dbContext, object erntity)
        {
            var entityType = erntity.GetType();
            return GetKeyProperties(dbContext, entityType).Select(x => erntity.GetUnproxiedType().GetProperty(x).GetValue(erntity)).ToArray();
        }

        /// <summary>
        /// Get an array of the object key values.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entity">The database object instance.</param>
        /// <returns>Returns an array of the object key values.</returns>
        public static object[] GetKeyValues<T>(this DbContext dbContext, T entity) where T : class
        {
            var entityType = typeof(T);
            return GetKeyProperties(dbContext, entityType).Select(x => typeof(T).GetProperty(x).GetValue(entity)).ToArray();
        }


        /// <summary>
        /// Checks if the entry already exists in database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entity">The object instance to check.</param>
        /// <returns>Returns true if the entry does not exists yet in database.</returns>
        public static bool IsNewEntry<T>(this DbContext dbContext, T entity) where T : class
        {
            if (dbContext.Model.FindEntityType(typeof(T).GetUnproxiedType()) == null)
            {
                return IsNewEntry(dbContext, (object)entity);
            }
            return dbContext.Set<T>().AsNoTracking().FirstOrDefault(BuildLambda<T>(dbContext.GetKeys<T>(), dbContext.GetKeyValues(entity))) == null;
        }

        /// <summary>
        /// Checks if the entry already exists in database.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entity">The object instance to check.</param>
        /// <returns>Returns true if the entry does not exists yet in database.</returns>
        public static bool IsNewEntry(this DbContext dbContext, object entity)
            => (bool)IsNewEntryMethod.MakeGenericMethod(entity.GetUnproxiedType()).Invoke(null, new object[] { dbContext, entity });


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


        private static Expression<Func<TEntity, bool>> BuildLambda<TEntity>(IReadOnlyList<IProperty> keyProperties, object[] keyValues) where TEntity : class
        {
            var entityParameter = Expression.Parameter(typeof(TEntity), "e");
            return Expression.Lambda<Func<TEntity, bool>>(BuildPredicate(keyProperties, keyValues, entityParameter), entityParameter);
        }

        private static BinaryExpression BuildPredicate(IReadOnlyList<IProperty> keyProperties, object[] keyValues, ParameterExpression entityParameter)
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