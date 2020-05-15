using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public class WebApplicationBase<
        TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
        TEnumsParent, TEnum, TValue,
        TStaticTextsParent, TStaticText,
        TLanguagesParent, TLanguage,
        TNavigationParent, TMenu> : ApplicationBase<
            TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
            TEnumsParent, TEnum, TValue,
            TStaticTextsParent, TStaticText,
            TLanguagesParent, TLanguage,
            TNavigationParent, TMenu>
        where TEntityTypesParent : WebEntityTypesParentBase<TEntity, TPropertiesParent, TProperty>
            where TEntity : WebEntityBase<TPropertiesParent, TProperty>, IWebEntity
                where TPropertiesParent : PropertiesParentBase<TProperty>
                    where TProperty : PropertyBase
        where TEnumsParent : EnumsParentBase<TEnum, TValue>
            where TEnum : EnumBase<TValue>
                where TValue : ValueBase
        where TStaticTextsParent : StaticTextsParentBase<TStaticText>
            where TStaticText : StaticTextBase
        where TLanguagesParent : LanguagesParentBase<TLanguage>
            where TLanguage : LanguageBase
        where TNavigationParent : NavigationParentBase<TMenu>
            where TMenu : MenuBase<TMenu>
    {
    }
}