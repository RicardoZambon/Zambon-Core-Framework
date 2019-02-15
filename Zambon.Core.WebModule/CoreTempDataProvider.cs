using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.WebModule
{
    class CoreTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context)
        {
            
            throw new NotImplementedException();
        }

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
            throw new NotImplementedException();
        }
    }
}
