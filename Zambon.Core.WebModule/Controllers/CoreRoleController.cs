using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zambon.Core.Security.BusinessObjects;

namespace Zambon.Core.WebModule.Controllers
{
    public class CoreRoleController<R> : CoreController<R> where R : Roles, new()
    {

        public CoreRoleController(CoreDbContext ctx, ApplicationService app) : base(ctx, app)
        {
            
        }
    }
}