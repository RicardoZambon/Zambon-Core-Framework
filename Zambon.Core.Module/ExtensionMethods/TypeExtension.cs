using System;
using System.ComponentModel;

namespace Zambon.Core.Module.ExtensionMethods
{
    public static class TypeExtension
    {

        public static string GetDefaultProperty(this Type _type)
        {
            var defaultProperties = _type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
            return defaultProperties.Length > 0 ? ((DefaultPropertyAttribute)defaultProperties[0]).Name : null;
        }

        //public static PropertyInfo GetPropertyRecursivelly(this Type _type, string _property)
        //{
        //    if (_property.IndexOf(".") >= 0)
        //    {
        //        var prop = _property.Substring(0, _property.IndexOf("."));
        //        _property = _property.Substring(_property.IndexOf(".") + 1, _property.Length - _property.IndexOf(".") - 1);

        //        return GetPropertyRecursivelly(_type.GetPropertyRecursivelly(prop).PropertyType, _property);
        //    }

        //    return _type.GetProperty(_property) ?? GetPropertyRecursivelly(_type.BaseType, _property);
        //}

    }
}