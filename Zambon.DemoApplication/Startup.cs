using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Zambon.Core.Database;
using Zambon.Core.Database.ChangeTracker.Services;
using Zambon.Core.Database.Services;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.WebModule.Configurations;
using Zambon.Core.WebModule.Services;

namespace Zambon.DemoApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //protected IConfigurationBuilder DefaultConfigurationBuilder() =>
        //    new ConfigurationBuilder()
        //        .SetBasePath(Env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{Env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //        .AddEnvironmentVariables();

        public virtual void ConfigureServices(IServiceCollection services)
        {
            //Database
            services.AddSingleton<CoreChangeTrackerManager>();
            services.AddScoped<CoreChangeTrackerInstance>();
            services.AddScoped<ICacheKeyService, WebCacheKeyService>();

            services.AddEntityFrameworkSqlServer();
            services.AddEntityFrameworkProxies();

            services.AddSingleton(new DbAdditionalAssemblies<CoreDbContext>(new WebAppModule().ReferencedModules(new List<Type>()).Select(x => x.Assembly.GetName().Name).Distinct()));

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<CoreDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(GetType().Assembly.GetName().Name).MigrationsHistoryTable("MigrationsHistory", "EF")
                );
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });


            //Module
            services.AddSingleton(typeof(IModule), typeof(WebAppModule));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<WebAppSettings>(Configuration.GetSection("AppSettings"));


            //WebModule
            services.AddSingleton(typeof(IModelProvider), typeof(WebModelProvider));


            //App
            services.AddSession();

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");

                //endpoints.MapDefaultControllerRoute();

                //endpoints.MapControllers();
                //endpoints.MapRazorPages();
            });
        }
    }
}