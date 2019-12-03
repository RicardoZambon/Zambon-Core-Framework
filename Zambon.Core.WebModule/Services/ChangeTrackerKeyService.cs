using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Zambon.Core.Database.ChangeTracker;
using Zambon.Core.Database.ChangeTracker.Services;

namespace Zambon.Core.WebModule.Services
{
    public class ChangeTrackerKeyService : IChangeTrackerKeyService
    {
        public const string CookieDatabaseKey = "DatabaseKey";

        public const string ClaimsUserId = "ID";

        public const string CookieFormKey = "DatabaseFormKey";

        #region Services

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructors

        public ChangeTrackerKeyService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        public ChangeTrackerKey RetrieveKey()
        {
            var databaseKey = Guid.NewGuid();
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieDatabaseKey))
            {
                databaseKey = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[CookieDatabaseKey].ToString());
            }
            else
            {
                StoreKey(databaseKey);
            }

            return new ChangeTrackerKey(databaseKey, Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimsUserId).Value));
        }

        public void StoreKey(Guid databaseKey)
        {
            if (!_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieDatabaseKey))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieDatabaseKey, Guid.NewGuid().ToString());
            }
        }

        #endregion
    }
}