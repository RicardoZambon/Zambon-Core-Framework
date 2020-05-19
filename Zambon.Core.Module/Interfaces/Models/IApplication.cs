using Zambon.Core.Database;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IApplication : ISerializationNode
    {
        void Validate<T>(CoreDbContext coreDbContext, AppSettings settings, T applicationModel) where T : IApplication;
    }

    public interface IApplication<TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews> : IApplication
        where TEntity : IEntity
        where TEnum : IEnum
        where TStaticText : IStaticText
        where TLanguage : ILanguage
        where TModuleConfigurations : IModuleConfigurations
        where TMenu : IMenu<TMenu>
        where TViews : IViews
    {
        #region XML Arrays

        ChildItemCollection<TEntity> EntityTypes { get; set; }

        ChildItemCollection<TEnum> Enums { get; set; }

        ChildItemCollection<TStaticText> StaticTexts { get; set; }

        ChildItemCollection<TLanguage> Languages { get; set; }

        ChildItemCollection<TMenu> Menus { get; set; }

        #endregion

        #region XML Elements

        TModuleConfigurations ModuleConfigurations { get; set; }

        TViews Views { get; set; }

        #endregion

        #region Methods

        void Validate<T>(CoreDbContext coreDbContext, AppSettings settings, T applicationModel) where T : IApplication, IApplication<TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews>;

        #endregion
    }
}