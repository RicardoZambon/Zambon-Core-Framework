using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Zambon.Core.Module.ExtensionMethods
{
    /// <summary>
    /// Extensions used in enums.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Get a list of all enum values.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>Return a dictionary with the enum number and its display name.</returns>
        public static Dictionary<int, string> GetEnumItems<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(k => Convert.ToInt32(k), v => v.GetEnumDisplayName());
        }

        /// <summary>
        /// Get the display name of a enum value.
        /// </summary>
        /// <param name="value">The enum item.</param>
        /// <returns>Return the enum display name from DisplayName attribute.</returns>
        public static string GetEnumDisplayName(this Enum value)
        {
            return value.GetType().GetMember(value.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name ?? value.ToString();
        }
    }
}