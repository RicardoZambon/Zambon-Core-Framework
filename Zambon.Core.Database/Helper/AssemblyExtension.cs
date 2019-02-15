using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Zambon.Core.Database.Helper
{
    public static class AssemblyExtension
    {
        public static IEnumerable<TypeInfo> GetTypesByInterface<I>(this Assembly assembly) where I : class
        {
            return assembly.DefinedTypes.Where(x => x.ImplementedInterfaces.Contains(typeof(I)));
        }

        public static IEnumerable<I> GetActualClasses<I>(this Assembly assembly, IEnumerable<TypeInfo> types) where I : class
        {
            var list = new List<I>();
            foreach (var type in types)
                if (!type.GetTypeInfo().IsAbstract && assembly.CreateInstance(type.FullName) is I entity)
                    list.Add(entity);
            return list;
        }
    }
}