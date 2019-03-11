using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.CustomProviders
{
    public interface IUserProvider
    {

        bool LogonAllowed(string username);

        Task<SignInResult> CheckPasswordAsync(string username, string password);

        Task<ClaimsPrincipal> CreatePrincipalAsync(string username);

    }
}