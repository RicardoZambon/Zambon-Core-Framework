using System;
using Zambon.Core.Module;
using Zambon.Core.WebModule.Model;

namespace Zambon.Core.WebModule
{
    public class WebMainModule : MainModule
    {
        public override Type ApplicationModelType => typeof(Application);

        public WebMainModule() : base()
        {

        }
    }
}