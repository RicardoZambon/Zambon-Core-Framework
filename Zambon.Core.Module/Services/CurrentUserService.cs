//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Zambon.Core.Database;
//using Zambon.Core.Module.BusinessObjects;
//using Zambon.Core.Module.Helper;

//namespace Zambon.Core.Module.Services
//{
//    public class CurrentUserService<U,R> : ICurrentUserService where U : BaseUser where R : BaseRole
//    {

//        #region Variables

//        private readonly IHttpContextAccessor _contextHttp;
//        private readonly CoreContext _ctx;

//        #endregion

//        #region Properties

//        public string CurrentIdentityName
//        {
//            get { return _contextHttp.HttpContext?.User?.Identity?.Name ?? string.Empty; }
//        }
//        public string CurrentUsername
//        {
//            get { return (CurrentIdentityName?.Length ?? 0) > 0 ? CurrentIdentityName.IndexOf("\\") > 0 ? CurrentIdentityName.Substring(CurrentIdentityName.IndexOf("\\") + 1, CurrentIdentityName.Length - CurrentIdentityName.IndexOf("\\") - 1) : CurrentIdentityName : string.Empty; }
//        }

//        BaseUser ICurrentUserService.CurrentUser => CurrentUser;

//        private U _CurrentUser;
//        public U CurrentUser
//        {
//            get
//            {
//                CheckUserChanged();
//                return _CurrentUser;
//            }
//        }

//        #endregion

//        #region Constructors

//        public CurrentUserService(IHttpContextAccessor contextHttp, CoreContext ctx)
//        {
//            _contextHttp = contextHttp;
//            _ctx = ctx;
//        }

//        #endregion

//        #region Methods

//        public void CheckUserChanged()
//        {
//            if ((_CurrentUser == null && !string.IsNullOrWhiteSpace(CurrentIdentityName)) || (_CurrentUser != null && _CurrentUser.Username != CurrentUsername))
//            {
//                var prefix = CurrentIdentityName.IndexOf("\\") > 0 ? CurrentIdentityName.Substring(0, CurrentIdentityName.IndexOf("\\") + 1) : string.Empty;

//                U user = default(U);
//                if (!string.IsNullOrWhiteSpace(CurrentIdentityName))
//                    user = _ctx.Set<U>().FirstOrDefault(
//                       x =>
//                           x.Username.ToLower() == CurrentUsername.ToLower()
//                           && ((!string.IsNullOrWhiteSpace(prefix) && (!string.IsNullOrWhiteSpace(x.UsernameDomainPrefix) ? x.UsernameDomainPrefix.ToUpper() : string.Empty) == prefix.ToUpper()) || string.IsNullOrWhiteSpace(prefix))
//                       );
//                _CurrentUser = user;
//            }
//            RefreshCurrentUserData();
//        }

//        private void RefreshCurrentUserData()
//        {
//            if (_CurrentUser != null)
//                _CurrentUser.FillSubordinatesArray(_ctx);
//        }

//        public bool HasRoleProperty(string propertyName)
//        {
//            return CurrentUser.HasRolePropertyDefined<R>(propertyName);
//        }

//        #endregion

//    }
//}