using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zambon.Core.Database;
using Zambon.Core.Database.Operations;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.Identity
{
    public class CoreUserStore<TUser> : /*ICoreUserStore<TUser>,*/ IUserPasswordStore<TUser>, IQueryableUserStore<TUser> where TUser : class, IUsers
    {
        private readonly CoreContext _ctx;

        public CoreUserStore(CoreContext ctx)
        {
            _ctx = ctx;
        }


        public IQueryable<TUser> Users => _ctx.Set<TUser>();

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            return CreateAsync(user, cancellationToken, true);
        }
        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken, bool useTransaction = true)
        {
            _ctx.SaveChanges(user, useTransaction);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            return DeleteAsync(user, cancellationToken, true);
        }
        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken, bool commitChanges = true)
        {
            _ctx.Delete<TUser>(user.ID, commitChanges);
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {

        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(userId);
            return Task.FromResult(_ctx.Find<TUser>(id));
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _ctx.Set<TUser>().FirstOrDefault(x => x.Username.ToUpper() == normalizedUserName.ToUpper());
            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ID.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!(string.IsNullOrWhiteSpace(user.Password)));
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Username = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            return UpdateAsync(user, cancellationToken, true);
        }
        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken, bool useTransaction = true)
        {
            _ctx.SaveChanges(user, useTransaction);
            return Task.FromResult(IdentityResult.Success);
        }

    }
}