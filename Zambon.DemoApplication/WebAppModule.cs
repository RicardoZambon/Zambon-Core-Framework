using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using Zambon.Core.Module.Model;
using Zambon.Core.WebModule;
using Zambon.DemoApplication.Domain;

namespace Zambon.DemoApplication
{
    public class WebAppModule : WebMainModule
    {
        public override List<Type> ReferencedModules(List<Type> models)
        {
            models.Add(typeof(AppModule));
            return base.ReferencedModules(models);
        }
    }
}