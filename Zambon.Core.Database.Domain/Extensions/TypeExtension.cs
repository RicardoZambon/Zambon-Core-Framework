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
    }
}