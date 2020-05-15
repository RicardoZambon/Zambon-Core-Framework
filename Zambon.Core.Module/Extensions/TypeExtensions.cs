using System;
using System.Reflection;

namespace Zambon.Core.Module.Extensions
{
    public static class TypeExtensions
    {
        public static PropertyInfo GetPropertyFromParents(this Type type, string propertyName)
        {
            PropertyInfo property = null;
            do
            {
                property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                type = type.BaseType;
            } while (property == null && type != typeof(object));
            return property;
        }
    }
}