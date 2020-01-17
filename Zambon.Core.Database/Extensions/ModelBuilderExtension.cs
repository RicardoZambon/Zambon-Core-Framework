using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Reflection;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Database.Extensions
{
    /// <summary>
    /// Provides additional methods to use when calling ModelBuilder.
    /// </summary>
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Applies configuration from all <see cref="IEntity" /> and <see cref="IConfigurableEntity" /> instances that are defined in provided assembly.
        /// </summary>
        /// <param name="modelBuilder">The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</param>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="predicate">Optional predicate to filter types within the assembly.</param>
        /// <returns>The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</returns>
        public static ModelBuilder EntitiesFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> predicate = null)
        {
            var entityMethod = typeof(ModelBuilder).GetMethods().Single(e => e.Name == nameof(ModelBuilder.Entity) && e.ContainsGenericParameters && e.GetParameters().Length == 0);
            var configureMethod = typeof(IConfigurableEntity).GetMethod(nameof(IConfigurableEntity.OnConfiguringEntity));
            foreach (var type in assembly.GetReferencedConstructibleTypes<IEntity>())
            {
                if (type.GetConstructor(Type.EmptyTypes) == null || (!predicate?.Invoke(type) ?? false))
                {
                    continue;
                }

                var typeBuilder = entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, null);
                if (type.ImplementsInterface<IConfigurableEntity>())
                {
                    configureMethod.Invoke(Activator.CreateInstance(type), new[] { typeBuilder });
                }

                typeof(ModelBuilderExtension).GetMethod(nameof(InitializeEntity)).MakeGenericMethod(type).Invoke(null, new[] { assembly, typeBuilder });
            }
            return modelBuilder;
        }

        /// <summary>
        /// Applies configuration from all <see cref="IQuery" /> and <see cref="IConfigurableQuery" /> instances that are defined in provided assembly.
        /// </summary>
        /// <param name="modelBuilder">The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</param>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="predicate">Optional predicate to filter types within the assembly.</param>
        /// <returns>The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</returns>
        public static ModelBuilder QueriesFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> predicate = null)
        {
            var queryMethod = typeof(ModelBuilder).GetMethods().Single(e => e.Name == nameof(ModelBuilder.Query) && e.ContainsGenericParameters && e.GetParameters().Length == 0);
            foreach (var type in assembly.GetReferencedConstructibleTypes<IQuery>())
            {
                if (type.GetConstructor(Type.EmptyTypes) == null || (!predicate?.Invoke(type) ?? false))
                {
                    continue;
                }
                queryMethod.MakeGenericMethod(type).Invoke(modelBuilder, null);
            }
            return modelBuilder;
        }


        /// <summary>
        /// Applies initialization from all <see cref="IDbInitializer" /> instances that are defined in provided assembly.
        /// </summary>
        /// <param name="modelBuilder">The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</param>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="predicate">Optional predicate to filter types within the assembly.</param>
        /// <returns>The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.</returns>
        public static ModelBuilder InitializeFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> predicate = null)
        {
            var seedMethod = typeof(IDbInitializer).GetMethod(nameof(IDbInitializer.Seed));
            foreach (var type in assembly.GetReferencedConstructibleTypes<IDbInitializer>())
            {
                if (type.GetConstructor(Type.EmptyTypes) == null || (!predicate?.Invoke(type) ?? false))
                {
                    continue;
                }
                seedMethod.Invoke(Activator.CreateInstance(type), new[] { modelBuilder });
            }
            return modelBuilder;
        }


        /// <summary>
        /// Initialize an entity using a generic initialize method.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="entityBuilder">Optional predicate to filter types within the assembly.</param>
        public static void InitializeEntity<T>(Assembly assembly, EntityTypeBuilder<T> entityBuilder) where T : class, IEntity
        {
            foreach (var type in assembly.GetReferencedConstructibleTypes<IDbInitializer<T>>())
            {
                type.GetMethod(nameof(IDbInitializer.Seed)).Invoke(Activator.CreateInstance(type), new[] { entityBuilder });
            }
        }
    }
}