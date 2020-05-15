using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Model.Entities;
using Zambon.Core.Module.Model.Enums;
using Zambon.Core.Module.Model.Languages;
using Zambon.Core.Module.Model.Navigation;
using Zambon.Core.Module.Model.Serialization;
using Zambon.Core.Module.Model.StaticTexts;
using Zambon.Core.Module.ModelAbstractions;

namespace Zambon.Core.Module.Model
{
    /// <summary>
    /// Represent the root XML node.
    /// </summary>
    [XmlRoot, XmlType(AnonymousType = true)]
    public class Application : ApplicationBase<EntityTypes, Entity>
    {
        //#region XML Attributes

        //#endregion

        //#region XML Elements

        //private EntityTypes _entityTypes;
        ///// <summary>
        ///// Represents a list of entity types in XML model file.
        ///// </summary>
        //[XmlElement(nameof(EntityTypes)), Browsable(false)]
        //public EntityTypes _EntityTypes
        //{
        //    get => _entityTypes;
        //    set => SetParent(value, ref _entityTypes);
        //}

        //private Enums.Enums _enums;
        ///// <summary>
        ///// Represents a list of enums in XML model file.
        ///// </summary>
        //[XmlElement(nameof(Model.Enums.Enums)), Browsable(false)]
        //public Enums.Enums _Enums
        //{
        //    get => _enums;
        //    set => SetParent(value, ref _enums);
        //}

        //private Languages.Languages _languages;
        ///// <summary>
        ///// Represents a list of languages in XML model file.
        ///// </summary>
        //[XmlElement(nameof(Model.Languages.Languages)), Browsable(false)]
        //public Languages.Languages _Languages
        //{
        //    get => _languages;
        //    set => SetParent(value, ref _languages);
        //}

        //private StaticTexts.StaticTexts _staticTexts;
        ///// <summary>
        ///// Represents a list of texts in XML model file.
        ///// </summary>
        //[XmlElement(nameof(Model.StaticTexts.StaticTexts)), Browsable(false)]
        //public StaticTexts.StaticTexts _StaticTexts
        //{
        //    get => _staticTexts;
        //    set => SetParent(value, ref _staticTexts);
        //}

        //private Configurations.ModuleConfigurations moduleConfigurations;
        ///// <summary>
        ///// Represents all configurations used by application in XML model file.
        ///// </summary>
        //[XmlElement(nameof(Configurations.ModuleConfigurations))]
        //public Configurations.ModuleConfigurations ModuleConfigurations
        //{
        //    get => moduleConfigurations;
        //    set => SetParent(value, ref moduleConfigurations);
        //}

        //private Navigation.Navigation navigation;
        ///// <summary>
        ///// Represents all navigation menus available in XML model file.
        ///// </summary>
        //public Navigation.Navigation Navigation
        //{
        //    get => navigation;
        //    set => SetParent(value, ref navigation);
        //}


        ////Views

        //#endregion

        //#region Properties

        ///// <summary>
        ///// Represents a list of entity types in XML model file.
        ///// </summary>
        //[XmlIgnore]
        //public ChildItemCollection<Entity> Entities => _EntityTypes?.EntitiesList ?? new ChildItemCollection<Entity>(null);

        ///// <summary>
        ///// Represents a list of enums in XML model file.
        ///// </summary>
        //[XmlIgnore]
        //public ChildItemCollection<Enum> Enums => _Enums?.EnumsList ?? new ChildItemCollection<Enum>(null);

        ///// <summary>
        ///// Represents a list of languages in XML model file.
        ///// </summary>
        //[XmlIgnore]
        //public ChildItemCollection<Language> Languages => _Languages?.LanguagesList ?? new ChildItemCollection<Language>(null);

        ///// <summary>
        ///// Represents a list of texts in XML model file.
        ///// </summary>
        //[XmlIgnore]
        //public ChildItemCollection<StaticText> StaticTexts => _StaticTexts?.TextsList ?? new ChildItemCollection<StaticText>(null);

        ///// <summary>
        ///// Represents all navigation menus available in XML model file.
        ///// </summary>
        //[XmlIgnore]
        //public ChildItemCollection<Menu> Menus => navigation?.Menus ?? new ChildItemCollection<Menu>(null);

        //#endregion
    }
}