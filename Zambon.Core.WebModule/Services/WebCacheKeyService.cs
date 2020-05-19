using Microsoft.AspNetCore.Http;
using System;
using Zambon.Core.Database.ChangeTracker.Services;

namespace Zambon.Core.WebModule.Services
{
    public class WebCacheKeyService : ICacheKeyService
    {
        #region Services

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructors

        public WebCacheKeyService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Overrides

        public Guid RetrieveDatabaseKey()
        {
            var databaseKey = Guid.NewGuid();
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("InstanceKey"))
            {
                databaseKey = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies["InstanceKey"].ToString());
            }
            else
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append("InstanceKey", databaseKey.ToString());
            }
            return databaseKey;
        }

        //TODO
        public Guid RetrieveFormKey()
            => Guid.Empty;

        public int RetrieveUserId()
            => Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("ID")?.Value ?? "-1");

        #endregion
    }
}