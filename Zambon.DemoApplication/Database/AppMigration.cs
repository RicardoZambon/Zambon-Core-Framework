//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Zambon.Core.Database;
//using Zambon.Core.Security;
//using Zambon.Core.Security.BusinessObjects;
//using Zambon.Core.Security.Identity;

//namespace Zambon.DemoApplication.Database
//{
//    public class AppMigration : IAppMigration
//    {
//        public void OnDataBaseCreated(CoreContext ctx)
//        {
//            var userManager = ctx.GetService<CoreUserManager<Users>>();
//            //var userManager = new CoreUserManager<Users>( new CoreUserStore<Users>(ctx), null, new PasswordHasher<Users>(), null, null, null, null, null, null);

//            var user = new Users() { AuthenticationType = Core.Module.Helper.Enums.AuthenticationType.UsernamePassword, Username = "admin", LogonAllowed = true };
//            userManager.CreateAsync(user, "admin");

//            var role = new Roles() { Name = "Administrators", IsAdministrative = true };
//            ctx.SaveChanges(role);

//            var roleUser = new RolesUsers() { UserId = user.ID, RoleId = role.ID };
//            ctx.SaveChanges(roleUser);


//        }
//    }
//}
