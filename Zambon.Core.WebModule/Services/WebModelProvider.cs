using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Languages;
using Zambon.Core.Module.Model.Nodes.Configurations;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;
using Zambon.Core.Module.Model.Nodes.Enums;
using Zambon.Core.Module.Model.Nodes.Languages;
using Zambon.Core.Module.Model.Nodes.Navigation;
using Zambon.Core.Module.Model.Nodes.StaticTexts;
using Zambon.Core.Module.Model.Nodes.Views;
using Zambon.Core.Module.Model.Nodes.Views.Buttons;
using Zambon.Core.Module.Model.Nodes.Views.Columns;
using Zambon.Core.Module.Model.Nodes.Views.GridTemplates;
using Zambon.Core.Module.Model.Nodes.Views.SearchProperties;
using Zambon.Core.WebModule.Model.Nodes;
using Zambon.Core.WebModule.Model.Nodes.Entities;

namespace Zambon.Core.WebModule.Services
{
    public class WebModelProvider : WebModelProviderBase<WebApplication,
        WebEntityTypesParent, WebEntity, PropertiesParent, Property,
        EnumsParent, Enum, Value,
        StaticTextsParent, StaticText,
        LanguagesParent, Language,
        ModuleConfigurationsParent,
        NavigationParent, Menu,
        ViewsParent, DetailView, ListView, LookupView,
            SearchPropertiesParent, SearchProperty,
            ButtonsParent, Button,
            ColumnsParent, Column,
            GridTemplatesParent, GridTemplate>
    {
        public WebModelProvider(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}