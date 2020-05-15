using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;
using Zambon.Core.Module.Model.Nodes.Navigation;
using Zambon.Core.Module.Model.Nodes.StaticTexts;
using Zambon.Core.WebModule.Model.Nodes;
using Zambon.Core.WebModule.Model.Nodes.Entities;

namespace Zambon.Core.WebModule.Services
{
    public class WebModelProvider : WebModelProviderBase<WebApplication, WebEntityTypesParent, WebEntity, PropertiesParent, Property, StaticTextsParent, StaticText, NavigationParent, Menu>
    {
        public WebModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}