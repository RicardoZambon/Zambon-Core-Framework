using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zambon.Core.Security.BusinessObjects;
using Zambon.Core.WebModule;

namespace Zambon.DemoApplication
{
    public class Startup : ModuleStartup
    {

        public Startup(IHostingEnvironment env)
        {
            Env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            CustomConfigureServices<Users,Roles>(services, Configuration);
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}