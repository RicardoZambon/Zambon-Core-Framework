using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Zambon.Core.Database
{
    public abstract class CoreContextFactory : IDesignTimeDbContextFactory<CoreContext>
    {
        public virtual CoreContext CreateDbContext(string[] args)
        {
            if (args.Length != 2)
                throw new Exception("Missing arguments for ConnectionString and/or MigrationsAssembly.");

            var optionsBuilder = new DbContextOptionsBuilder<CoreContext>()
                .UseSqlServer(args[0], b => b.MigrationsAssembly(args[1]).MigrationsHistoryTable("MigrationsHistory", "EF"));

            return new CoreContext(optionsBuilder.Options);
        }
    }
}