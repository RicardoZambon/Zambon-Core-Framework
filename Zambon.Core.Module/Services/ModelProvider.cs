using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Languages;
using Zambon.Core.Module.Model.Nodes;
using Zambon.Core.Module.Model.Nodes.Configurations;
using Zambon.Core.Module.Model.Nodes.Entities;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;
using Zambon.Core.Module.Model.Nodes.Enums;
using Zambon.Core.Module.Model.Nodes.Languages;
using Zambon.Core.Module.Model.Nodes.Navigation;
using Zambon.Core.Module.Model.Nodes.StaticTexts;
using Zambon.Core.Module.Model.Views;

namespace Zambon.Core.Module.Services
{
    public class ModelProvider : BaseModelProvider<Application,
        EntityTypesParent, Entity, PropertiesParent, Property,
        EnumsParent, Enum, Value,
        StaticTextsParent, StaticText,
        LanguagesParent, Language,
        ModuleConfigurationsParent,
        NavigationParent, Menu,
        ViewsParent>
    {
        public ModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}