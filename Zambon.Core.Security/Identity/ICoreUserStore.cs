using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zambon.Core.Security.Identity
{
    public interface ICoreUserStore<TUser> : IUserStore<TUser> where TUser : class
    {

        Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken, bool useTransaction = true);
        
        Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken, bool commitChanges = true);
        
        Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken, bool useTransaction = true);

    }
}
