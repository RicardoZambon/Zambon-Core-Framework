using Microsoft.AspNetCore.Http;
using System;
using Zambon.Core.Database.ChangeTracker;
using Zambon.Core.Database.ChangeTracker.Services;

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

            var userId = Convert.ToInt32(httpContextAccessor.HttpContext?.User.FindFirst("ID").Value);
            return new InstanceKey(databaseKey, userId);
        }

        public void StoreKey(Guid databaseInstanceKey)
        {
            if (!httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("InstanceKey"))
                httpContextAccessor.HttpContext.Response.Cookies.Append("InstanceKey", Guid.NewGuid().ToString());
        }
    }
}