using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;
using Zambon.Core.Database.Cache.Services;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.Services
{
    public class ChangeTrackerService : IInstanceKeyService
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentUserService currentUserService;

        public ChangeTrackerService(IHttpContextAccessor _httpContextAccessor, ICurrentUserService _currentUserService)
        {
            httpContextAccessor = _httpContextAccessor;
            currentUserService = _currentUserService;
        }


        public InstanceKey RetrieveKey()
        {
            var databaseKey = Guid.NewGuid();
            if (httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("InstanceKey"))
                databaseKey = Guid.Parse(httpContextAccessor.HttpContext.Request.Cookies["InstanceKey"].ToString());
            else
                StoreKey(databaseKey);

            return new InstanceKey(databaseKey, currentUserService.CurrentUser.ID);
        }

        public void StoreKey(Guid databaseInstanceKey)
        {
            if (!httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("InstanceKey"))
                httpContextAccessor.HttpContext.Response.Cookies.Append("InstanceKey", Guid.NewGuid().ToString());
        }
    }
}