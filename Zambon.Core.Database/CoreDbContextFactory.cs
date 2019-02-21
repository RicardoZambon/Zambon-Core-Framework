using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Zambon.Core.Database
{
    /// <summary>
    /// A factory for creating derived Microsoft.EntityFrameworkCore.DbContext instances. Implement this interface to enable design-time services for context types that do not have a public default constructor. At design-time, derived Microsoft.EntityFrameworkCore.DbContext instances can be created in order to enable specific design-time experiences such as Migrations. Design-time services will automatically discover implementations of this interface that are in the startup assembly or the same assembly as the derived context.
    /// </summary>
    public abstract class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        /// <summary>
        /// Creates a new instance of a derived context.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of CoreDbContext.</returns>
        public virtual CoreDbContext CreateDbContext(string[] args)
        {
            if (args.Length != 2)
                throw new Exception("Missing arguments for ConnectionString and/or MigrationsAssembly.");

            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>()
                .UseSqlServer(args[0], b => b.MigrationsAssembly(args[1]).MigrationsHistoryTable("MigrationsHistory", "EF"));

            return new CoreDbContext(optionsBuilder.Options);
        }
    }
}