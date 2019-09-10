using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Zambon.Core.Module;
using Zambon.Core.Module.DI;

namespace Zambon.Core.WebModule
{
    public class ModuleStartup
    {

        protected IHostingEnvironment Env { get; set; }

        protected IConfigurationBuilder DefaultConfigurationBuilder() =>
            new ConfigurationBuilder()
                .SetBasePath(Env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

        public IConfigurationRoot Configuration { get; protected set; }


        protected virtual bool UseDataProtectionInFileSystem => !Env.IsDevelopment();

        protected virtual bool UseHttps => false;
        protected virtual TimeSpan HstsMaxAge => TimeSpan.FromDays(60);
        protected virtual int HttpsPort => 5001;
        protected virtual int HttpsRedirectStatusCode => StatusCodes.Status308PermanentRedirect;

        protected virtual bool UseDatabaseDistributedCache => !Env.IsDevelopment();

        protected virtual TimeSpan LoginCookieExpireTimeSpan => new TimeSpan(336, 0, 0);


        public ModuleStartup(IHostingEnvironment env)
        {
            Env = env;
            Configuration = DefaultConfigurationBuilder().Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                if (UseHttps)
                    app.UseHsts();
            }

            app.UseSession();
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new EmbeddedFileProvider(typeof(ModuleStartup).GetTypeInfo().Assembly, "Zambon.Core.WebModule.wwwroot")
            });

            app.ConfigureLocalization();

            app.UseMvc(routes =>
            {
                routes.ConfigureMvcLocalizationRoute(app);

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            if (UseHttps)
                app.UseHttpsRedirection();
        }
    }
}