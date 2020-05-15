using System;
using Zambon.Core.Module;
using Zambon.Core.WebModule.Model;
using Zambon.Core.WebModule.Model.Nodes;

namespace Zambon.Core.WebModule
{
    public class WebMainModule : MainModule
    {
        public override Type ApplicationModelType => typeof(WebApplication);

        public WebMainModule() : base()
        {

        }
    }
}