using System;
using System.ComponentModel;

namespace Zambon.Core.Module.ExtensionMethods
{
    /// <summary>
    /// Extension types used for Type.
    /// </summary>
    public static class TypeExtension
    {

        /// <summary>
        /// Retrieve the default property name from DefaultPropertyAttribute.
        /// </summary>
        /// <param name="type">The type to search for.</param>
        /// <returns>If found, will return the default property name; otherwise, return null.</returns>
        public static string GetDefaultProperty(this Type type)
        {
            var defaultProperties = type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
            return defaultProperties.Length > 0 ? ((DefaultPropertyAttribute)defaultProperties[0]).Name : null;
        }

    }
}