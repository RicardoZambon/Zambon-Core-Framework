using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Languages;
using Zambon.Core.Module.Model.Nodes.Configurations;
using Zambon.Core.Module.Model.Nodes.Entities;
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

namespace Zambon.Core.Module.Model.Nodes
{
    [XmlRoot]
    public class Application : ApplicationBase<
        EntityTypesParent, Entity, PropertiesParent, Property,
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