//using Microsoft.AspNetCore.Http;
//using Zambon.Core.Database.ChangeTracker;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Zambon.Core.WebModule.Helper
//{
//    public static class ChangeTrackerHelper
//    {

//        public static ChangeTrackerInstanceKey GetInstanceKey(HttpContext httpContext)
//        {
//            if (httpContext.Request.Cookies["InstanceKey"] == null)
//                throw new Exception("Missing instance key!");

//            string currentUserName = httpContext?.User?.Identity?.Name ?? string.Empty;
//            if (currentUserName == null)
//                throw new Exception("Missing user identity!");

//            return new ChangeTrackerInstanceKey(Guid.Parse(httpContext.Request.Cookies["InstanceKey"].ToString()), currentUserName);
//        }

//    }
//}
