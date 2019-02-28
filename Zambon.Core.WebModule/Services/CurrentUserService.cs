using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Security.BusinessObjects;

namespace Zambon.Core.WebModule.Services
{
    public class CurrentUserService<TUser,TRole> : IUserService where TUser : class, IUsers where TRole : class, IRoles
    {

        #region Variables

        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly CoreDbContext Ctx;

        #endregion

        #region Properties

        private string _currentIdentityName;
        public string CurrentIdentityName
        {
            get { if (_currentIdentityName == null) _currentIdentityName = HttpContextAccessor.HttpContext?.User?.Identity?.Name; return _currentIdentityName; }
        }

        IUsers IUserService.CurrentUser => CurrentUser;

        private TUser _currentUser;
        public TUser CurrentUser
        {
            get
            {
                if (_currentUser == null && CurrentIdentityName != null)
                    CheckUserChanged();
                return _currentUser;
            }
        }

        #endregion

        #region Constructors

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, CoreDbContext ctx)
        {
            HttpContextAccessor = httpContextAccessor;
            Ctx = ctx;
        }

        #endregion

        #region Methods

        public virtual void CheckUserChanged()
        {
            if (!string.IsNullOrWhiteSpace(CurrentIdentityName))
            {
                var user = Ctx.Set<TUser>().FirstOrDefault(x => string.Equals(x.Username, CurrentIdentityName, StringComparison.InvariantCultureIgnoreCase));
                _currentUser = user;

                RefreshCurrentUserData();
            }
        }

        private void RefreshCurrentUserData()
        {
            //if (_currentUser != null)
            //    _currentUser.FillSubordinatesArray(_ctx);
        }

        #endregion

    }
}