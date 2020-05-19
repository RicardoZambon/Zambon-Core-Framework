using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Configurations;
using Zambon.Core.Module.Model.Nodes.EntityTypes;
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
    public sealed class Application : ApplicationBase<Entity<Property>, Enum<Value>, StaticText, Language, ModuleConfigurations, Menu,
        Views<
            DetailView<Button>,
            ListView<SearchProperty, Button, Column, GridTemplate>,
            LookupView<SearchProperty, Column, GridTemplate>
        >>
    {
        #region Constructors

        public Application() : base()
        {
        }

        public Application(CoreDbContext coreDbContext) : base(coreDbContext)
        {
        }

        #endregion
    }
}