using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Entities;
using Zambon.Core.Module.Enums;
using Zambon.Core.Module.Languages;
using Zambon.Core.Module.Navigation;
using Zambon.Core.Module.Serialization;
using Zambon.Core.Module.StaticTexts;

namespace Zambon.Core.Module
{
    /// <summary>
    /// Represent the root XML node.
    /// </summary>
    [XmlRoot]
    public class Application : BaseNode
    {
        #region XML Attributes

        #endregion

        #region XML Elements

        private EntityTypes _entityTypes;
        /// <summary>
        /// Represents a list of entity types in XML model file.
        /// </summary>
        [XmlElement(nameof(EntityTypes)), Browsable(false)]
        public EntityTypes _EntityTypes
        {
            get => _entityTypes;
            set => SetParent(value, ref _entityTypes);
        }

        private Enums.Enums _enums;
        /// <summary>
        /// Represents a list of enums in XML model file.
        /// </summary>
        [XmlElement(nameof(Module.Enums.Enums)), Browsable(false)]
        public Enums.Enums _Enums {
            get => _enums;
            set => SetParent(value, ref _enums);
        }

        private Languages.Languages _languages;
        /// <summary>
        /// Represents a list of languages in XML model file.
        /// </summary>
        [XmlElement(nameof(Module.Languages.Languages)), Browsable(false)]
        public Languages.Languages _Languages
        {
            get => _languages;
            set => SetParent(value, ref _languages);
        }

        private StaticTexts.StaticTexts _staticTexts;
        /// <summary>
        /// Represents a list of texts in XML model file.
        /// </summary>
        [XmlElement(nameof(Module.StaticTexts.StaticTexts)), Browsable(false)]
        public StaticTexts.StaticTexts _StaticTexts {
            get => _staticTexts;
            set => SetParent(value, ref _staticTexts);
        }

        private Configurations.ModuleConfigurations moduleConfigurations;
        /// <summary>
        /// Represents all configurations used by application in XML model file.
        /// </summary>
        [XmlElement(nameof(Configurations.ModuleConfigurations))]
        public Configurations.ModuleConfigurations ModuleConfigurations
        {
            get => moduleConfigurations;
            set => SetParent(value, ref moduleConfigurations);
        }

        private Navigation.Navigation navigation;
        /// <summary>
        /// Represents all navigation menus available in XML model file.
        /// </summary>
        public Navigation.Navigation Navigation
        {
            get => navigation;
            set => SetParent(value, ref navigation);
        }


        //Views

        #endregion

        #region Properties

        /// <summary>
        /// Represents a list of entity types in XML model file.
        /// </summary>
        [XmlIgnore]
        public ChildItemCollection<Entity> Entities => _EntityTypes?.EntitiesList ?? new ChildItemCollection<Entity>(null);

        /// <summary>
        /// Represents a list of enums in XML model file.
        /// </summary>
        [XmlIgnore]
        public ChildItemCollection<Enum> Enums => _Enums?.EnumsList ?? new ChildItemCollection<Enum>(null);

        /// <summary>
        /// Represents a list of languages in XML model file.
        /// </summary>
        [XmlIgnore]
        public ChildItemCollection<Language> Languages => _Languages?.LanguagesList ?? new ChildItemCollection<Language>(null);

        /// <summary>
        /// Represents a list of texts in XML model file.
        /// </summary>
        [XmlIgnore]
        public ChildItemCollection<StaticText> StaticTexts => _StaticTexts?.TextsList ?? new ChildItemCollection<StaticText>(null);

        /// <summary>
        /// Represents all navigation menus available in XML model file.
        /// </summary>
        [XmlIgnore]
        public ChildItemCollection<Menu> Menus => navigation?.Menus ?? new ChildItemCollection<Menu>(null);

        #endregion
    }
}