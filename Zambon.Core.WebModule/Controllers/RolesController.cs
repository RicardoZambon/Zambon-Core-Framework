using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Security;
using Zambon.Core.Security.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zambon.Core.Security.BusinessObjects;

namespace Zambon.Core.WebModule.Controllers
{
    public class RolesController : CoreRoleController<Roles>
    {
        public RolesController(CoreDbContext ctx, ApplicationService app) : base(ctx, app)
        {

        }
    }
}