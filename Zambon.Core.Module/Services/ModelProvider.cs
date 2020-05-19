using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Nodes;
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

namespace Zambon.Core.Module.Services
{
    public class ModelProvider : ModelProviderBase<
        Application,
            Entity<Property>, Enum<Value>, StaticText, Language, ModuleConfigurations, Menu,
            Views<
                DetailView<Button>,
                ListView<SearchProperty, Button, Column, GridTemplate>,
                LookupView<SearchProperty, Column, GridTemplate>
            >
        >
    {
        public ModelProvider(DbContextOptions dbOptions, IOptions<AppSettings> appSettings, IModule mainModule) : base(dbOptions, appSettings, mainModule)
        {
        }
    }
}