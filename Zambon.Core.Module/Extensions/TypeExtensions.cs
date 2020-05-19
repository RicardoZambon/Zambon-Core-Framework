using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Zambon.Core.Module.Extensions
{
    public static class TypeExtensions
    {
        //public static PropertyInfo GetPropertyFromParents(this Type type, string propertyName)
        //{
        //    PropertyInfo property;
        //    do
        //    {
        //        property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        //        type = type.BaseType;
        //    } while (property == null && type != typeof(object));
        //    return property;
        //}

        public static string GetDisplayName(this Type type)
            => type.GetCustomAttribute<DisplayAttribute>()?.Name
                ?? type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                ?? Regex.Replace(type.Name, "(\\B[A-Z])", " $1");
    }
}