using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;
using Zambon.Core.Database.Cache.Services;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.Services
{
    public class ChangeTrackerService : IInstanceKeyService
    {

        private readonly IHttpContextAccessor httpContextAccessor;

        public ChangeTrackerService(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }


        public InstanceKey RetrieveKey()
        {
            var databaseKey = Guid.NewGuid();
            if (httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("InstanceKey"))
                databaseKey = Guid.Parse(httpContextAccessor.HttpContext.Request.Cookies["InstanceKey"].ToString());
            else
                StoreKey(databaseKey);

            var userId = Convert.ToInt32(httpContextAccessor.HttpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            return new InstanceKey(databaseKey, userId);
        }

        public void StoreKey(Guid databaseInstanceKey)
        {
            if (!httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("InstanceKey"))
                httpContextAccessor.HttpContext.Response.Cookies.Append("InstanceKey", Guid.NewGuid().ToString());
        }
    }
}