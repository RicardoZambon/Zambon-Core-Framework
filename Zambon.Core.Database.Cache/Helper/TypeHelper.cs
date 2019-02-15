using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Database.Cache.Helper
{
    public static class TypeHelper
    {

        public static string GetCorrectTypeName(this Type _type)
        {
            return GetCorrectType(_type).Name;
        }

        public static Type GetCorrectType(this Type _type)
        {
            if (_type.FullName.StartsWith("Castle.Proxies."))
                return _type.BaseType;
            return _type;
        }

    }
}
