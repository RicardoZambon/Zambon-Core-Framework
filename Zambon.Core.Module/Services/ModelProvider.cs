using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Nodes;
using Zambon.Core.Module.Model.Nodes.Entities;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;

namespace Zambon.Core.Module.Services
{
    public class ModelProvider : BaseModelProvider<Application, EntityTypes, Entity, Properties, Property>
    {
        public ModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}