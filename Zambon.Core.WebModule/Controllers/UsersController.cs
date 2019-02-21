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
    public class UsersController : CoreUserController<Users>
    {
        public UsersController(CoreDbContext ctx, CoreUserManager<Users> coreUserManager, ApplicationService app) : base(ctx, coreUserManager, app)
        {

        }
    }
}