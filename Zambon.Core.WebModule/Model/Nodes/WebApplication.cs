using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Configurations;
using Zambon.Core.Module.Model.Nodes.EntityTypes;
using Zambon.Core.Module.Model.Nodes.Enums;
using Zambon.Core.Module.Model.Nodes.Languages;
using Zambon.Core.Module.Model.Nodes.Navigation;
using Zambon.Core.Module.Model.Nodes.StaticTexts;
using Zambon.Core.Module.Model.Nodes.Views;
using Zambon.Core.Module.Model.Nodes.Views.Columns;
using Zambon.Core.Module.Model.Nodes.Views.SearchProperties;
using Zambon.Core.WebModule.Model.Nodes.Entities;
using Zambon.Core.WebModule.Model.Nodes.Views.Buttons;
using Zambon.Core.WebModule.Model.Nodes.Views.GridTemplates;

namespace Zambon.Core.WebModule.Model.Nodes
{
    [XmlRoot(APPLICATION_NODE)]
    public sealed class WebApplication : ApplicationBase<WebEntity<Property>, Enum<Value>, StaticText, Language, ModuleConfigurations, Menu,
        Views<
            DetailView<WebButton>,
            ListView<SearchProperty, WebButton, Column, WebGridTemplate>,
            LookupView<SearchProperty, Column, WebGridTemplate>
        >>
    {
    }
}