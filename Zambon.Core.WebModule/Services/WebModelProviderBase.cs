using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.Services
{
    public abstract class WebModelProviderBase<TApplication> : ModelProviderBase<TApplication> where TApplication : class, IApplication, new()
    {
        public WebModelProviderBase(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}