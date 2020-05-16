using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ApplicationBase
        <TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
        TEnumsParent, TEnum, TValue,
        TStaticTextsParent, TStaticText,
        TLanguagesParent, TLanguage,
        TModuleConfigurationsParent,
        TNavigationParent, TMenu>
        : SerializeNodeBase, IApplication
            <TEntityTypesParent, TEntity, TPropertiesParent, TProperty,
            TEnumsParent, TEnum, TValue,
            TStaticTextsParent, TStaticText,
            TLanguagesParent, TLanguage,
            TModuleConfigurationsParent,
            TNavigationParent, TMenu>

        where TEntityTypesParent : EntityTypesParentBase<TEntity, TPropertiesParent, TProperty>
            where TEntity : EntityBase<TPropertiesParent, TProperty>
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
    {
        #region Constants

        protected const string APPLICATION_NODE = "Application";

        private const string ENTITY_TYPES_NODE = "EntityTypes";

        private const string NAVIGATION_NODE = "Navigation";

        #endregion

        #region XML Elements

        private TEntityTypesParent _entityTypes;
        [XmlElement(ENTITY_TYPES_NODE), Browsable(false)]
        public TEntityTypesParent _EntityTypes
        {
            get => _entityTypes;
            set => SetParent(value, ref _entityTypes);
        }

        private TEnumsParent _enums;
        [XmlElement(nameof(Enums)), Browsable(false)]
        public TEnumsParent _Enums
        {
            get => _enums;
            set => SetParent(value, ref _enums);
        }

        private TStaticTextsParent _staticTexts;
        [XmlElement(nameof(StaticTexts)), Browsable(false)]
        public TStaticTextsParent _StaticTexts
        {
            get => _staticTexts;
            set => SetParent(value, ref _staticTexts);
        }

        private TLanguagesParent _languages;
        [XmlElement(nameof(Languages)), Browsable(false)]
        public TLanguagesParent _Languages
        {
            get => _languages;
            set => SetParent(value, ref _languages);
        }

        private TModuleConfigurationsParent moduleConfigurations;
        [XmlElement]
        public TModuleConfigurationsParent ModuleConfigurations
        {
            get => moduleConfigurations;
            set => SetParent(value, ref moduleConfigurations);
        }

        private TNavigationParent _navigation;
        [XmlElement(NAVIGATION_NODE), Browsable(false)]
        public TNavigationParent _Navigation
        {
            get => _navigation;
            set => SetParent(value, ref _navigation);
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public ChildItemCollection<TEntity> Entities => _EntityTypes?.EntitiesList ?? new ChildItemCollection<TEntity>(null);

        [XmlIgnore]
        public ChildItemCollection<TEnum> Enums => _Enums?.EnumsList ?? new ChildItemCollection<TEnum>(null);

        [XmlIgnore]
        public ChildItemCollection<TStaticText> StaticTexts => _StaticTexts?.StaticTextsList ?? new ChildItemCollection<TStaticText>(null);

        [XmlIgnore]
        public ChildItemCollection<TLanguage> Languages => _Languages?.LanguagesList ?? new ChildItemCollection<TLanguage>(null);

        [XmlIgnore]
        public ChildItemCollection<TMenu> Menus => _Navigation?.MenusList ?? new ChildItemCollection<TMenu>(null);

        #endregion
    }
}