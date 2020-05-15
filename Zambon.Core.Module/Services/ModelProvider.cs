using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Nodes;
using Zambon.Core.Module.Model.Nodes.Entities;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;
using Zambon.Core.Module.Model.Nodes.Navigation;
using Zambon.Core.Module.Model.Nodes.StaticTexts;

namespace Zambon.Core.Module.Services
{
    public class ModelProvider : BaseModelProvider<Application, EntityTypesParent, Entity, PropertiesParent, Property, StaticTextsParent, StaticText, NavigationParent, Menu>
    {
        public ModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}