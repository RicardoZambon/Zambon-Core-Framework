using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IApplication
        <TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
        TEnumsParent, TEnum, TValue,
        TStaticTextsParent, TStaticText,
        TLanguagesParent, TLanguage,
        TModuleConfigurationsParent,
        TNavigationParent, TMenu,
        TViewsParent>

        where TEntityTypesParent : IEntityTypesParent<TEntity, TPropertiesParent, TProperty>
            where TEntity : IEntity<TPropertiesParent, TProperty>
                where TPropertiesParent : IPropertiesParent<TProperty>
                    where TProperty : IProperty
        where TEnumsParent : IEnumsParent<TEnum, TValue>
            where TEnum : IEnum<TValue>
                where TValue : IValue
        where TStaticTextsParent : IStaticTextsParent<TStaticText>
            where TStaticText : IStaticText
        where TLanguagesParent : ILanguagesParent<TLanguage>
            where TLanguage : ILanguage
        where TModuleConfigurationsParent : IModuleConfigurationsParent
        where TNavigationParent : INavigationParent<TMenu>
            where TMenu : IMenu<TMenu>
        where TViewsParent : IViewsParent
    {
        #region XML Elements

        TEntityTypesParent _EntityTypes { get; set; }

        TEnumsParent _Enums { get; set; }

        TStaticTextsParent _StaticTexts { get; set; }

        TLanguagesParent _Languages { get; set; }

        TModuleConfigurationsParent ModuleConfigurations { get; set; }

        TNavigationParent _Navigation { get; set; }

        TViewsParent Views { get; set; }

        #endregion

        #region Properties

        ChildItemCollection<TEntity> Entities { get; }

        ChildItemCollection<TEnum> Enums { get; }

        ChildItemCollection<TStaticText> StaticTexts { get; }

        ChildItemCollection<TLanguage> Languages { get; }

        ChildItemCollection<TMenu> Menus { get; }

        #endregion
    }
}