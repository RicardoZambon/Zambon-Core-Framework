using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Nodes.Configurations;
using Zambon.Core.Module.Model.Nodes.EntityTypes;
using Zambon.Core.Module.Model.Nodes.Enums;
using Zambon.Core.Module.Model.Nodes.Languages;
using Zambon.Core.Module.Model.Nodes.Navigation;
using Zambon.Core.Module.Model.Nodes.StaticTexts;
using Zambon.Core.Module.Model.Nodes.Views;
using Zambon.Core.Module.Model.Nodes.Views.Columns;
using Zambon.Core.Module.Model.Nodes.Views.SearchProperties;
using Zambon.Core.WebModule.Model.Nodes;
using Zambon.Core.WebModule.Model.Nodes.Entities;
using Zambon.Core.WebModule.Model.Nodes.Views.Buttons;
using Zambon.Core.WebModule.Model.Nodes.Views.GridTemplates;

namespace Zambon.Core.WebModule.Services
{
    public class WebModelProvider : WebModelProviderBase<
        WebApplication,
            WebEntity<Property>, Enum<Value>, StaticText, Language, ModuleConfigurations, Menu,
            Views<
                DetailView<WebButton>,
                ListView<SearchProperty, WebButton, Column, WebGridTemplate>,
                LookupView<SearchProperty, Column, WebGridTemplate>
            >
        >
    {
        public WebModelProvider(DbContextOptions dbOptions, IOptions<AppSettings> appSettings, IModule mainModule) : base(dbOptions, appSettings, mainModule)
        {
        }
    }
}