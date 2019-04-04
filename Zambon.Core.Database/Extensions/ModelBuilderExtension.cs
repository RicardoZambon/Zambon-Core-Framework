using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Database.ExtensionMethods
{
    /// <summary>
    /// Provides additional methods to use when calling ModelBuilder.
    /// </summary>
    public static class ModelBuilderExtension
    {
        /// <summary>
        ///     Applies configuration from all <see cref="IEntity" /> and <see cref="IConfigurableEntity" /> instances that are defined in provided assembly.
        /// </summary>
        /// <param name="modelBuilder">The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</param>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="predicate">Optional predicate to filter types within the assembly.</param>
        /// <returns>The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</returns>
        public static ModelBuilder EntitiesFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> predicate = null)
        {
            var entityMethod = typeof(ModelBuilder).GetMethods().Single(e => e.Name == "Entity" && e.ContainsGenericParameters && e.GetParameters().Length == 0);
            var configureMethod = typeof(IConfigurableEntity).GetMethod("Configure");
            foreach (var type in assembly.GetReferencedConstructibleTypes<IEntity>())
            {
                if (type.GetConstructor(Type.EmptyTypes) == null || (!predicate?.Invoke(type) ?? false))
                    continue;

                var typeBuilder = entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, null);
                if (type.ImplementsInterface<IConfigurableEntity>())
                    configureMethod.Invoke(Activator.CreateInstance(type), new[] { typeBuilder });
            }

            return modelBuilder;
        }

        /// <summary>
        ///     Applies configuration from all <see cref="IQuery" /> and <see cref="IConfigurable" /> instances that are defined in provided assembly.
        /// </summary>
        /// <param name="modelBuilder">The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</param>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="predicate">Optional predicate to filter types within the assembly.</param>
        /// <returns>The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</returns>
        public static ModelBuilder QueriesFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> predicate = null)
        {
            var queryMethod = typeof(ModelBuilder).GetMethods().Single(e => e.Name == "Query" && e.ContainsGenericParameters && e.GetParameters().Length == 0);
            var configureMethod = typeof(IConfigurableQuery).GetMethod("Configure");
            foreach (var type in assembly.GetReferencedConstructibleTypes<IQuery>())
            {
                if (type.GetConstructor(Type.EmptyTypes) == null || (!predicate?.Invoke(type) ?? false))
                    continue;

                var typeBuilder = queryMethod.MakeGenericMethod(type).Invoke(modelBuilder, null);
                if (type.ImplementsInterface<IConfigurableQuery>())
                    configureMethod.Invoke(Activator.CreateInstance(type), new[] { typeBuilder });
            }

            return modelBuilder;
        }
    }
}