using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zambon.Core.Database.ExtensionMethods
{
    /// <summary>
    /// Helper extension to get and instantiate the objects when calling OnConfigureMethod in CoreDbContext.
    /// </summary>
    public static class AssemblyExtension
    {

        /// <summary>
        /// Search for the specified type implementation within the assembly.
        /// </summary>
        /// <typeparam name="I">The type that should search.</typeparam>
        /// <param name="assembly">The assembly to search.</param>
        /// <returns>Returns a list of TypeInfo, if no implementation were found, will return a blank list.</returns>
        public static IEnumerable<TypeInfo> GetTypesByInterface<I>(this Assembly assembly) where I : class
        {
            return assembly.DefinedTypes.Where(x => x.ImplementedInterfaces.Contains(typeof(I)));
        }

        /// <summary>
        /// Gets the instanced elements for the informed types.
        /// </summary>
        /// <typeparam name="I">The type that should create the instance.</typeparam>
        /// <param name="assembly">The original assembly.</param>
        /// <param name="types">List of types.</param>
        /// <returns>Returns a list of instanced objects.</returns>
        private static IEnumerable<I> GetActualClasses<I>(this Assembly assembly, IEnumerable<TypeInfo> types) where I : class
        {
            var list = new List<I>();
            foreach (var type in types)
                if (!type.GetTypeInfo().IsAbstract && assembly.CreateInstance(type.FullName) is I entity)
                    list.Add(entity);
            return list;
        }

        /// <summary>
        /// Search in the assembly and referenced assemblies for a specific type.
        /// </summary>
        /// <typeparam name="I">Type to search.</typeparam>
        /// <param name="rootAssembly">Parent assembly to search.</param>
        /// <returns>Returns a list of instanced objects.</returns>
        public static List<I> GetReferencedClasses<I>(this Assembly rootAssembly) where I : class
        {
            var list = new List<I>();

            list.AddRange(rootAssembly.GetActualClasses<I>(rootAssembly.GetTypesByInterface<I>()));

            foreach (var assembly in rootAssembly.GetReferencedAssemblies())
            {
                var dll = Assembly.Load(assembly);

                var types = dll.GetTypesByInterface<I>();
                var classes = dll.GetActualClasses<I>(types);
                list.AddRange(classes);

                dll = null;
            }
            return list;
        }
    }
}