using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Security.BusinessObjects;
using Zambon.Core.Security.Identity;

namespace Zambon.Core.WebModule.CustomProviders
{
    public class UserProvider<TUser> : IUserProvider where TUser : class, IUsers
    {

        private readonly CoreUserManager<TUser> _userManager;

        private readonly SignInManager<TUser> _signInManager;


        public UserProvider(CoreUserManager<TUser> userManager, SignInManager<TUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public bool LogonAllowed(string username)
        {
            return _userManager.Users.Any(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToUpper() == username.ToUpper() && x.LogonAllowed);
        }

        public async Task<SignInResult> CheckPasswordAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public async Task<ClaimsPrincipal> CreatePrincipalAsync(string username)
        {
            return await _signInManager.CreateUserPrincipalAsync(await _userManager.FindByNameAsync(username)); ;
        }

    }
}