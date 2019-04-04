using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Zambon.Core.Database.DI
{
    /// <summary>
    /// Database injection methods to configure the database.
    /// </summary>
    public static class DatabaseInjection
    {
        /// <summary>
        /// Extension method to configure the database.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="migrationsAssemblyName">The main project running assembly name.</param>
        /// <param name="migrationsTableName">The table name to use in migrations assembly.</param>
        /// <param name="migrationsSchema">The schema name to use in migrations assembly.</param>
        public static void AddDatabase(this IServiceCollection services, string connectionString, string migrationsAssemblyName, string migrationsTableName = "MigrationsHistory", string migrationsSchema = "EF")
        {
            if (migrationsTableName == null)
                migrationsTableName = "MigrationsHistory";

            if (migrationsSchema == null)
                migrationsSchema = "EF";

            services.AddEntityFrameworkSqlServer();
            services.AddEntityFrameworkProxies();

            services.AddDbContextPool<CoreDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(migrationsAssemblyName).MigrationsHistoryTable(migrationsTableName, migrationsSchema)
                );
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });
        }
    }
}