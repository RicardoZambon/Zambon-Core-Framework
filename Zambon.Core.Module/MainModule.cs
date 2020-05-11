using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database;

namespace Zambon.Core.Module
{
    public partial class MainModule
    {
        private readonly CoreDbContext _ctx;

        public MainModule(CoreDbContext ctx)
        {
            this._ctx = ctx;
        }

    }
}