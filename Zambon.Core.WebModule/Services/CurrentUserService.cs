using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zambon.Core.Database;
using Zambon.Core.Module.BusinessObjects;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Security.BusinessObjects;

namespace Zambon.Core.WebModule.Services
{
    public class CurrentUserService<TUser,TRole> : ICurrentUserService where TUser : Users where TRole : Roles
    {

        #region Variables

        private readonly IHttpContextAccessor _contextHttp;
        private readonly CoreContext _ctx;

        #endregion

        #region Properties

        private string _currentIdentityName;
        public string CurrentIdentityName
        {
            get { if (_currentIdentityName == null) _currentIdentityName = _contextHttp.HttpContext?.User?.Identity?.Name; return _currentIdentityName; }
        }

        IUsers ICurrentUserService.CurrentUser => CurrentUser;

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

        public CurrentUserService(IHttpContextAccessor contextHttp, CoreContext ctx)
        {
            _contextHttp = contextHttp;
            _ctx = ctx;
        }

        #endregion

        #region Methods

        public virtual void CheckUserChanged()
        {
            if (!string.IsNullOrWhiteSpace(CurrentIdentityName))
            {
                var user = _ctx.Set<TUser>().FirstOrDefault(x => string.Equals(x.Username, CurrentIdentityName, StringComparison.InvariantCultureIgnoreCase));
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