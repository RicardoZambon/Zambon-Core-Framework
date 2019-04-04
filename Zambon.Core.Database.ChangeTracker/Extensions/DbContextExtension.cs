using Microsoft.EntityFrameworkCore;
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
        internal static IReadOnlyList<IProperty> GetKeys<T>(this DbContext ctx) where T : class
        {
            var entityType = ctx.Model.FindEntityType(typeof(T).GetUnproxiedType());
            return entityType.FindPrimaryKey().Properties;
        }

        internal static object[] GetKeyValues<T>(this DbContext ctx, T entity) where T : class
        {
            var entityType = ctx.Model.FindEntityType(typeof(T).GetUnproxiedType());
            var key = entityType.FindPrimaryKey();

            var keys = new object[key.Properties.Count];
            for (var i = 0; i < key.Properties.Count; i++)
                keys[i] = typeof(T).GetProperty(key.Properties[i].Name).GetValue(entity);
            return keys;
        }

        /// <summary>
        /// Checks if the entry already exists in database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="ctx">The database context instance.</param>
        /// <param name="entity">The object instance to check.</param>
        /// <returns>Returns true if the entry does not exists yet in database.</returns>
        public static bool IsNewEntry<T>(this DbContext ctx, T entity) where T : class
            => ctx.Set<T>().AsNoTracking().FirstOrDefault(BuildLambda<T>(ctx.GetKeys<T>(), ctx.GetKeyValues(entity))) == null;

        /// <summary>
        /// Creates a Microsoft.EntityFrameworkCore.DbSet`1 that can be used to query and save instances of TEntity.
        /// </summary>
        /// <param name="ctx">The database context instance.</param>
        /// <param name="type">The type of entity for which a set should be returned.</param>
        /// <returns>A set for the given entity type.</returns>
        public static IQueryable Set(this DbContext ctx, Type type)
            => (IQueryable)ctx.GetType().GetMethod("Set").MakeGenericMethod(type).Invoke(ctx, null);
        


        private static Expression<Func<TEntity, bool>> BuildLambda<TEntity>(IReadOnlyList<IProperty> keyProperties, object[] keyValues) where TEntity : class
        {
            var entityParameter = Expression.Parameter(typeof(TEntity), "e");
            return Expression.Lambda<Func<TEntity, bool>>(BuildPredicate(keyProperties, keyValues, entityParameter), entityParameter);
        }

        private static BinaryExpression BuildPredicate(IReadOnlyList<IProperty> keyProperties, object[] keyValues, ParameterExpression entityParameter)
        {
            var predicate = GenerateEqualExpression(keyProperties[0], 0);
            for (var i = 1; i < keyProperties.Count; i++)
                predicate = Expression.AndAlso(predicate, GenerateEqualExpression(keyProperties[i], i));
            return predicate;

            BinaryExpression GenerateEqualExpression(IProperty property, int i) => Expression.Equal(Expression.Property(entityParameter, property.Name), Expression.Constant(keyValues[i]));
        }
    }
}