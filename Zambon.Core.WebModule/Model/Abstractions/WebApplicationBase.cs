using Zambon.Core.Database;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebApplicationBase<TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews> : ApplicationBase<TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews>
        where TEntity : class, IEntity, IWebEntity
        where TEnum : class, IEnum
        where TStaticText : class, IStaticText
        where TLanguage : class, ILanguage
        where TModuleConfigurations : class, IModuleConfigurations
        where TMenu : class, IMenu<TMenu>
        where TViews : class, IViews
    {
        #region Constructors

        public WebApplicationBase() : base()
        {
        }

        public WebApplicationBase(CoreDbContext coreDbContext) : base(coreDbContext)
        {
        }

        #endregion
    }
}