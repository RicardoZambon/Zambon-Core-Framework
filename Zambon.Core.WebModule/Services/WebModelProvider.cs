using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.WebModule.Model.Nodes;

namespace Zambon.Core.WebModule.Services
{
    public class WebModelProvider : WebModelProviderBase<WebApplication>
    {
        public WebModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}