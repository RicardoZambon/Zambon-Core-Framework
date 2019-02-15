using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Zambon.Core.Module.Helper
{
    public static class TypeExtension
    {
        //public static bool IsNumeticType(this Type _type)
        //{
        //    if (_type.IsGenericType && _type.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        return IsNumeticType(Nullable.GetUnderlyingType(_type));

        //    switch (Type.GetTypeCode(_type))
        //    {
        //        case TypeCode.Byte:
        //        case TypeCode.SByte:
        //        case TypeCode.UInt16:
        //        case TypeCode.UInt32:
        //        case TypeCode.UInt64:
        //        case TypeCode.Int16:
        //        case TypeCode.Int32:
        //        case TypeCode.Int64:
        //        case TypeCode.Decimal:
        //        case TypeCode.Double:
        //        case TypeCode.Single:
        //            return true;
        //        default:
        //            return false;
        //    }
        //}

        //public static bool IsDateType(this Type _type)
        //{
        //    if (_type.IsGenericType && _type.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        return IsDateType(Nullable.GetUnderlyingType(_type));

        //    switch (Type.GetTypeCode(_type))
        //    {
        //        case TypeCode.DateTime:
        //            return true;
        //        default:
        //            return false;
        //    }
        //}


        public static string GetDefaultProperty(this Type _type)
        {
            var defaultProperties = _type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
            return defaultProperties.Length > 0 ? ((DefaultPropertyAttribute)defaultProperties[0]).Name : null;
        }

        public static PropertyInfo GetPropertyRecursivelly(this Type _type, string _property)
        {
            if (_property.IndexOf(".") >= 0)
            {
                var prop = _property.Substring(0, _property.IndexOf("."));
                _property = _property.Substring(_property.IndexOf(".") + 1, _property.Length - _property.IndexOf(".") - 1);

                return GetPropertyRecursivelly(_type.GetPropertyRecursivelly(prop).PropertyType, _property);
            }

            return _type.GetProperty(_property) ?? GetPropertyRecursivelly(_type.BaseType, _property);
        }

    }
}