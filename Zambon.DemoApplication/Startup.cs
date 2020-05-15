using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.WebModule.Configurations;
using Zambon.Core.WebModule.Services;

namespace Zambon.DemoApplication
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; protected set; }

        protected IHostingEnvironment Env { get; set; }


        public Startup(IHostingEnvironment env)
        {
            Env = env;
            Configuration = DefaultConfigurationBuilder().Build();
        }


        protected IConfigurationBuilder DefaultConfigurationBuilder() =>
            new ConfigurationBuilder()
                .SetBasePath(Env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();


        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IModule), typeof(WebAppModule));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<WebAppSettings>(Configuration.GetSection("AppSettings"));

            services.AddSingleton(typeof(IModelProvider), typeof(WebModelProvider));
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var modelProvider = (WebModelProvider)app.ApplicationServices.GetService<IModelProvider>();

            var model = modelProvider.GetModel("pt-br");
            if (model != null)
            {
                ReadEntityTypes(model._EntityTypes);
            }
        }


        public void ReadEntityTypes<TEntity, TProperties, TProperty>(IEntityTypesParent<TEntity, TProperties, TProperty> entityTypes)
            where TEntity : IEntity<TProperties, TProperty>
                where TProperties : IPropertiesParent<TProperty>
                    where TProperty : IProperty
        {
            foreach (var entity in entityTypes.EntitiesList)
            {
                var id = entity.Id;
                var displayName = entity.DisplayName;

                Console.WriteLine($"ID: {id}. Display Name: {displayName}.");
            }
        }
    }
}