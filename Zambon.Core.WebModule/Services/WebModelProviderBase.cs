using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.Model.Abstractions;

namespace Zambon.Core.WebModule.Services
{
    public abstract class WebModelProviderBase<TApplication,
        TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
        TEnumsParent, TEnum, TValue,
        TStaticTextsParent, TStaticText,
        TLanguagesParent, TLanguage,
        TModuleConfigurationsParent,
        TNavigationParent, TMenu,
        TViewsParent, TDetailView, TListView, TLookupView,
            TSearchPropertiesParent, TSearchProperty,
            TButtonsParent, TButton,
            TColumnsParent, TColumn,
            TGridTemplatesParent, TGridTemplate>
        : BaseModelProvider<TApplication,
            TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
            TEnumsParent, TEnum, TValue,
            TStaticTextsParent, TStaticText,
            TLanguagesParent, TLanguage,
            TModuleConfigurationsParent,
            TNavigationParent, TMenu,
            TViewsParent, TDetailView, TListView, TLookupView,
                TSearchPropertiesParent, TSearchProperty,
                TButtonsParent, TButton,
                TColumnsParent, TColumn,
                TGridTemplatesParent, TGridTemplate>

        where TApplication : WebApplicationBase<
            TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
            TEnumsParent, TEnum, TValue,
            TStaticTextsParent, TStaticText,
            TLanguagesParent, TLanguage,
            TModuleConfigurationsParent,
            TNavigationParent, TMenu,
            TViewsParent, TDetailView, TListView, TLookupView,
                TSearchPropertiesParent, TSearchProperty,
                TButtonsParent, TButton,
                TColumnsParent, TColumn,
                TGridTemplatesParent, TGridTemplate>, new()
        where TEntityTypesParent : WebEntityTypesParentBase<TEntity, TPropertiesParent, TProperty>
            where TEntity : WebEntityBase<TPropertiesParent, TProperty>
                where TPropertiesParent : PropertiesParentBase<TProperty>
                    where TProperty : PropertyBase
        where TEnumsParent : EnumsParentBase<TEnum, TValue>
            where TEnum : EnumBase<TValue>
                where TValue : ValueBase
        where TStaticTextsParent : StaticTextsParentBase<TStaticText>
            where TStaticText : StaticTextBase
        where TLanguagesParent : LanguagesParentBase<TLanguage>
            where TLanguage : LanguageBase
        where TModuleConfigurationsParent : ModuleConfigurationsParentBase
        where TNavigationParent : NavigationParentBase<TMenu>
            where TMenu : MenuBase<TMenu>
        where TViewsParent : WebViewsParentBase<TDetailView, TListView, TLookupView, TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
            where TDetailView : WebDetailViewBase<TButtonsParent, TButton>
            where TListView : WebListViewBase<TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
            where TLookupView : WebLookupViewBase<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>

            where TSearchPropertiesParent : SearchPropertiesParentBase<TSearchProperty>
                where TSearchProperty : SearchPropertyBase
            where TButtonsParent : WebButtonsParentBase<TButton>
                where TButton : WebButtonBase
            where TColumnsParent : ColumnsParentBase<TColumn>
                where TColumn : ColumnBase
            where TGridTemplatesParent : WebGridTemplatesParentBase<TGridTemplate>
                where TGridTemplate : WebGridTemplateBase
    {
        public WebModelProviderBase(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}