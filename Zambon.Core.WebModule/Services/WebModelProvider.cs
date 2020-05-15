using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.Model.Nodes;
using Zambon.Core.WebModule.Model.Nodes.Entities;

namespace Zambon.Core.WebModule.Services
{
    public class WebModelProvider : BaseModelProvider<WebApplication, WebEntityTypes, WebEntity, Properties, Property>
    {
        public WebModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}