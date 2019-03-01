using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Zambon.Core.Module.ExtensionMethods
{
    public static class EnumExtension
    {
        public static Dictionary<int, string> GetEnumItems<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(k => Convert.ToInt32(k), v => v.GetEnumDisplayName());
        }

        public static string GetEnumDisplayName(this Enum value)
        {
            return value.GetType().GetMember(value.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name ?? value.ToString();
        }
    }
}