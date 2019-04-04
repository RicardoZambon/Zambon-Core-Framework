using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Database.Domain.Models;

namespace Zambon.Core.Database.Domain.DI
{
    public static class AutoMapperStartup
    {
        public static void ConfigureAutoMapper(this IServiceCollection services, params string[] referencesAssemblies)
        {
            services.AddAutoMapper(configAction =>
            {
                foreach (var assemblyName in referencesAssemblies)
                {
                    var assembly = Assembly.Load(assemblyName);
                    foreach (var type in assembly.GetReferencedConstructibleTypes<IModel>())
                    {
                        if (type.GetConstructor(Type.EmptyTypes) == null)
                            continue;

                        configAction.CreateMap(type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAutoMapperModel<>)).GenericTypeArguments[0], type).ReverseMap();
                    }
                }
            });
        }
    }
}