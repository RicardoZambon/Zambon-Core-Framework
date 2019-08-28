using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Zambon.Core.WebModule.DI
{
    public static class FileProvidersInjection
    {
        public static void AddCustomFileProviders(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(
                    new CompositeFileProvider(new EmbeddedFileProvider(typeof(FileProvidersInjection).GetTypeInfo().Assembly, "Zambon.Core.WebModule"))
                );
            });
        }
    }
}