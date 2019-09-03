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
using Zambon.Core.Module;
using Zambon.Core.Module.DI;
using Zambon.Core.WebModule;
using Zambon.Core.WebModule.DI;
using Zambon.Core.WebModule.Services;

namespace Zambon.DemoApplication
{
    public class Startup : ModuleStartup
    {
        
        public Startup(IHostingEnvironment env) : base(env)
        {
        }


        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRequestLocalization();

            base.Configure(app, env, loggerFactory);
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            #region Database

            //services.AddAutoMapperToModels(GetType().Assembly.GetName().Name, "Test App", "Zambon.Core.WebModule");

            //services.AddChangeTrackerServices<ChangeTrackerService>();
            //services.AddChangeTrackerDbCache(options =>
            //{
            //    options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            //    options.SchemaName = "Cache";
            //    options.TableName = "CachedData";
            //    options.DefaultSlidingExpiration = new System.TimeSpan(14, 0, 0, 0);
            //});
            //services.AddDatabase(Configuration.GetConnectionString("DefaultConnection"), GetType().Assembly.GetName().Name, additionalAssemblies: new[] { "Test" });

            #endregion

            services.AddHttpContextAccessor();

            #region Module

            services.AddCoreConfigs<CoreConfigs>(Configuration);

            //if (!Env.IsDevelopment())
            //{
            //    services.AddDataProtection(Configuration, Env.ApplicationName);
            //}

            services.AddApplicationModel();
            LocalizationInjection.AddLocalization(services);

            #endregion

            #region Security

            //services.AddIdentityAuthentication<Users, Roles>(Env.ApplicationName);
            //services.AddUserService<LoggedUserService<Users, Roles>>();

            #endregion

            #region WebModule

            services.AddSession();

            //services.AddApplicationCookie(Env.ApplicationName);

            services.AddCustomFileProviders();

            services.AddMvc(config =>
                {
                    config.ModelMetadataDetailsProviders.Add(new CoreMetaDataProvider());

                    //var policy = new AuthorizationPolicyBuilder()
                    //     .RequireAuthenticatedUser()
                    //     .Build();
                    //config.Filters.Add(new AuthorizeFilter(policy));
                })
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2)
            .AddSessionStateTempDataProvider();

            //services.AddCustomValidators();

            #endregion

            //CustomConfigureServices(services, Configuration);
        }
    }
}