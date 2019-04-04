//using Microsoft.Extensions.DependencyInjection;
//using System.Linq;
//using System.Reflection;
//using Zambon.Core.Database.Domain.Contracts.Repositories;
//using Zambon.Core.Database.Domain.Extensions;

//namespace Zambon.Core.Database.DI
//{
//    public static class RepositoriesStartup
//    {
//        public static void ConfigureRepositories(this IServiceCollection services, params string[] referencesAssemblies)
//        {
//            foreach (var assemblyName in referencesAssemblies)
//            {
//                var assembly = Assembly.Load(assemblyName);
//                foreach (var type in assembly.GetReferencedConstructibleTypes<IRepository>())
//                {
//                    if (!type.IsAbstract)
//                    {
//                        var allInterfaces = type.GetInterfaces();
//                        var minimalInterfaces = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces()));
//                        services.AddTransient(minimalInterfaces.FirstOrDefault() ?? type, type);
//                    }
//                }
//            }
//        }
//    }
//}