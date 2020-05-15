using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model;

namespace Zambon.Core.Module.Services
{
    public class ModelProvider : BaseModelProvider<Application>
    {
        public ModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}