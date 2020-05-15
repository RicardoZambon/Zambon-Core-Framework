using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.Model;

namespace Zambon.Core.WebModule.Services
{
    public class WebModelProvider : BaseModelProvider<Application>
    {
        public WebModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}