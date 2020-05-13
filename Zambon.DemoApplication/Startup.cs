using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;

namespace Zambon.DemoApplication
{
    public class Startup// : ModuleStartup
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

            services.AddSingleton(typeof(ModelStore));
            
            services.AddSingleton(typeof(IModelService), typeof(ModelService<WebAppModule>));
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var modelService = app.ApplicationServices.GetService<IModelService>();

            modelService.GetModel("en");
        }
    }
}