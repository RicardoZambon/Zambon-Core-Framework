using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Services
{
    public abstract class WebModelProviderBase<TApplication, TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews> : ModelProviderBase<TApplication, TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews>
        where TApplication : class, IApplication<TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews>, new()
        where TEntity : class, IEntity, IWebEntity, new()
        where TEnum : class, IEnum
        where TStaticText : class, IStaticText
        where TLanguage : class, ILanguage
        where TModuleConfigurations : class, IModuleConfigurations
        where TMenu : class, IMenu<TMenu>
        where TViews : class, IViews
    {
        public WebModelProviderBase(DbContextOptions dbOptions, IOptions<AppSettings> appSettings, IModule mainModule) : base(dbOptions, appSettings, mainModule)
        {
        }
    }
}