using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zambon.Core.Database;
using Zambon.Core.WebModule;

namespace Zambon.DemoApplication
{
    public class ApplicationContextFactory : WebContextFactory
    {
        public override CoreContext CreateDbContext(string[] args)
        {
            return base.CreateDbContext(new[] { GetType().Assembly.GetName().Name });
        }
    }
}
