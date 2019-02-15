using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Zambon.Core.Module.Helper
{
    public static class EnumExtension
    {
        public static Dictionary<int, string> GetEnumItems<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(k => Convert.ToInt32(k), v => Enum.GetName(typeof(T), v));
        }

        public static object GetEnumDisplayName(object value)
        {
            return value.GetType().GetMember(value.ToString())?.First()?.GetCustomAttribute<DisplayAttribute>()?.Name ?? value;
        }
    }
}