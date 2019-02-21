using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Zambon.Core.Database;

namespace Zambon.Core.WebModule
{
    public class WebContextFactory : CoreDbContextFactory
    {
        public override CoreDbContext CreateDbContext(string[] args)
        {
            if (args.Length != 1)
                throw new Exception("Missing arguments for MigrationsAssembly.");

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            var configuration = builder.Build();

            return base.CreateDbContext(new[] { configuration.GetConnectionString("DefaultConnection"), args [0] });
        }
    }
}