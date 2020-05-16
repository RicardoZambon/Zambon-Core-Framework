using System.Xml.Serialization;
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
using Zambon.Core.WebModule.Model.Abstractions;
using Zambon.Core.WebModule.Model.Nodes.Entities;

namespace Zambon.Core.WebModule.Model.Nodes
{
    [XmlRoot(APPLICATION_NODE)]
    public class WebApplication : WebApplicationBase<
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
    }
}