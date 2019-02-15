using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zambon.Core.Database;
using Zambon.Core.Database.Operations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Security.BusinessObjects;

namespace Zambon.Core.Security.Identity
{
    public class CoreRoleStore<TRole> : IRoleStore<TRole>, IQueryableRoleStore<TRole> where TRole : class, IRoles
    {
        private readonly CoreContext _ctx;

        public CoreRoleStore(CoreContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<TRole> Roles => _ctx.Set<TRole>();

        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            return CreateAsync(role, cancellationToken, true);
        }
        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken, bool useTransaction = true)
        {
            _ctx.SaveChanges(role, useTransaction);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            return DeleteAsync(role, cancellationToken, true);
        }
        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken, bool commitChanges = true)
        {
            _ctx.Delete<TRole>(role.ID, commitChanges);
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {

        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(roleId);
            return Task.FromResult(_ctx.Find<TRole>(id));
        }

        public Task<TRole> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var role = _ctx.Set<TRole>().FirstOrDefault(x => x.Name.ToUpper() == normalizedUserName.ToUpper());
            return Task.FromResult(role);
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.ID.ToString());
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.Name = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            return UpdateAsync(role, cancellationToken, true);
        }
        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken, bool useTransaction = true)
        {
            _ctx.SaveChanges(role, useTransaction);
            return Task.FromResult(IdentityResult.Success);
        }

    }
}