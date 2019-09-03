using Zambon.Core.Database.Domain.Attributes;
using Zambon.Core.Database.Domain.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace Zambon.Core.Database.Domain.Extensions
{
    /// <summary>
    /// Helper methods to use when having Castle.Proxies entities.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Check if a type implements any interface
        /// </summary>
        /// <param name="type">The type to search for the interface.</param>
        /// <param name="interfaceType">The interface type that should search.</param>
        /// <returns>If the type implements the interface, returns true.</returns>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
            => type.GetTypeInfo().ImplementedInterfaces.Contains(interfaceType);

        /// <summary>
        /// Check if a type implements any interface
        /// </summary>
        /// <typeparam name="I">The interface type that should search.</typeparam>
        /// <param name="type">The type to search for the interface.</param>
        /// <returns>If the type implements the interface, returns true.</returns>
        public static bool ImplementsInterface<I>(this Type type) where I : class
            => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(I));


        /// <summary>
        /// Get the entity type the model is related to.
        /// </summary>
        /// <param name="type">The model type.</param>
        /// <returns>Returns the entity type.</returns>
        public static Type GetModelEntityType(this Type type)
        {
            if (type.ImplementsInterface<IBaseObject>())
            {
                return type;
            }
            return type.GetInterfaces().LastOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IModel<>))?.GenericTypeArguments[0];
        }


        /// <summary>
        /// Returns the entity database query defined in property attribute.
        /// </summary>
        /// <param name="type">The entity type.</param>
        /// <param name="property">The property name</param>
        /// <param name="args">Arguments for the query.</param>
        /// <returns>Returns the database query as string.</returns>
        public static string GetEntityDbQuery(this Type type, string property, out string[] args)
        {
            if (type.GetProperty(property).GetCustomAttributes(typeof(DbQueryAttribute), true)?.FirstOrDefault() is DbQueryAttribute query)
            {
                args = query.Args;
                return query.Query;
            }
            args = new string[0];
            return null;
        }
    }
}