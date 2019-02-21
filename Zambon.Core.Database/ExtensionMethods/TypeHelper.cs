using System;
using System.Linq;
using System.Reflection;

namespace Zambon.Core.Database.ExtensionMethods
{
    /// <summary>
    /// Helper methods to use when having Castle.Proxies entities.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Returns the correct type name if using "Caste.Proxies".
        /// </summary>
        /// <param name="_type">Type to detect.</param>
        /// <returns>Returns the string type name.</returns>
        public static string GetCorrectTypeName(this Type _type)
        {
            return GetCorrectType(_type).Name;
        }

        /// <summary>
        /// Returns the correct type if using "Caste.Proxies".
        /// </summary>
        /// <param name="_type">Type to search for.</param>
        /// <returns>Returns the type.</returns>
        public static Type GetCorrectType(this Type _type)
        {
            if (_type.FullName.StartsWith("Castle.Proxies."))
                return _type.BaseType;
            return _type;
        }

        /// <summary>
        /// Check if a type implements any interface
        /// </summary>
        /// <typeparam name="I">The interface type that should search.</typeparam>
        /// <param name="type">The type to search for the interface.</param>
        /// <returns>If the type implements the interface, returns true.</returns>
        public static bool ImplementsInterface<I>(this Type type) where I : class
        {
            return type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(I));
        }
    }
}