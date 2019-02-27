using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.Identity
{
    public class CoreRoleManager<TRole> : RoleManager<TRole> where TRole : class
    {
        public CoreRoleManager(
            IRoleStore<TRole> store,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {

        }
    }
}