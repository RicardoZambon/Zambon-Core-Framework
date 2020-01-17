using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zambon.Core.Database.Domain.Extensions;

namespace Zambon.Core.Database.Extensions
{
    /// <summary>
    /// Extension methods to get and instantiate the objects when calling OnConfigureMethod in CoreDbContext.
    /// </summary>
    public static class AssembliesExtension
    {
        /// <summary>
        /// Search for the specified type implementation within the assembly.
        /// </summary>
        /// <typeparam name="I">The type that should search.</typeparam>
        /// <param name="assembly">The assembly to search.</param>
        /// <returns>Returns a list of TypeInfo, if no implementation were found, will return a blank list.</returns>
        public static IEnumerable<TypeInfo> GetTypesByInterface<I>(this Assembly assembly) where I : class
            => assembly.DefinedTypes.Where(x => x.ImplementedInterfaces.Contains(typeof(I)));


        /// <summary>
        /// Search in the assembly and referenced assemblies for a specific type.
        /// </summary>
        /// <param name="rootAssembly">Parent assembly to search.</param>
        /// <param name="searchedType">Type to search.</param>
        /// <returns>Returns a list of instanced objects.</returns>
        public static IEnumerable<TypeInfo> GetReferencedConstructibleTypes(this Assembly rootAssembly, Type searchedType)
        {
            var list = new List<TypeInfo>();
            list.AddRange(rootAssembly.GetConstructibleTypes(searchedType));
            foreach (var assembly in rootAssembly.GetReferencedAssemblies())
            {
                list.AddRange(Assembly.Load(assembly).GetConstructibleTypes(searchedType));
            }
            return list;
        }

        /// <summary>
        /// Search in the assembly and referenced assemblies for a specific type.
        /// </summary>
        /// <typeparam name="I">Type to search.</typeparam>
        /// <param name="rootAssembly">Parent assembly to search.</param>
        /// <returns>Returns a list of instanced objects.</returns>
        public static IEnumerable<TypeInfo> GetReferencedConstructibleTypes<I>(this Assembly rootAssembly) where I : class
            => rootAssembly.GetConstructibleTypes<I>();


        /// <summary>
        /// Get all types from the assembly that are not abstract and have no generic type definition.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <param name="searchedType">The type searched.</param>
        /// <returns>Returns an IEnumerable of TypeInfo.</returns>
        public static IEnumerable<TypeInfo> GetConstructibleTypes(this Assembly assembly, Type searchedType)
            => assembly.GetLoadableDefinedTypes().Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition && t.ImplementsInterface(searchedType));

        /// <summary>
        /// Get all types from the assembly that are not abstract and have no generic type definition.
        /// </summary>
        /// <typeparam name="T">The type searched.</typeparam>
        /// <param name="assembly">The assembly to search.</param>
        /// <returns>Returns an IEnumerable of TypeInfo.</returns>
        public static IEnumerable<TypeInfo> GetConstructibleTypes<T>(this Assembly assembly) where T : class
            => assembly.GetLoadableDefinedTypes().Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition && t.ImplementsInterface<T>());


        /// <summary>
        /// Get all defined types from the assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <returns>Returns an IEnumerable of TypeInfo.</returns>
        public static IEnumerable<TypeInfo> GetLoadableDefinedTypes(this Assembly assembly)
        {
            try
            {
                return assembly.DefinedTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null).Select(IntrospectionExtensions.GetTypeInfo);
            }
        }
    }
}