using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.Identity
{
    public class ClaimsPrincipalFactory<U, R> : UserClaimsPrincipalFactory<U, R> where U : class, IUsers where R : class, IRoles
    {

        public ClaimsPrincipalFactory(UserManager<U> userManager, RoleManager<R> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(U user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("ID", (user?.ID ?? 0).ToString()));
            return identity;
        }

    }
}