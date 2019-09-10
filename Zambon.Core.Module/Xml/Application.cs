using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Zambon.Core.Module.Xml.Configuration;
using Zambon.Core.Module.Xml.Languages;
using Zambon.Core.Module.Xml.StaticTexts;

namespace Zambon.Core.Module.Xml
{
    /// <summary>
    /// Represents the root class from ApplicationModel XML.
    /// </summary>
    [XmlRoot]
    public class Application : XmlNode
    {
        #region XML Attributes

        /// <summary>
        /// The application name.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the application.
        /// </summary>
        [XmlAttribute("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Display name to use in menu header, if empty will then use the [Name] attribute.
        /// </summary>
        [XmlAttribute("MenuName")]
        public string MenuName { get; set; }

        #endregion

        #region XML Elements

        ///// <summary>
        ///// Element representation of the XML <EntityTipes></EntityTipes> node.
        ///// </summary>
        //[XmlElement("EntityTypes"), Browsable(false)]
        //public EntityTypesArray _EntityTypes { get; set; }

        ///// <summary>
        ///// Element representation of the XML <Models></Models> node.
        ///// </summary>
        //[XmlElement("Models"), Browsable(false)]
        //public ModelsArray _Models { get; set; }

        ///// <summary>
        ///// Element representation of the XML <Enums></Enums> node.
        ///// </summary>
        //[XmlElement("Enums"), Browsable(false)]
        //public EnumsArray _Enums { get; set; }

        /// <summary>
        /// Element representation of the XML <StaticTexts></StaticTexts> node.
        /// </summary>
        [XmlElement("StaticTexts"), Browsable(false)]
        public StaticTextsArray _StaticTexts { get; set; }


        /// <summary>
        /// Element representation of the XML <Languages></Languages> node.
        /// </summary>
        [XmlElement("Languages"), Browsable(false)]
        public LanguagesArray _Languages { get; set; }


        /// <summary>
        /// Module default configuration.
        /// </summary>
        [XmlElement("ModuleConfiguration")]
        public ModuleConfiguration ModuleConfiguration { get; set; }


        ///// <summary>
        ///// Element representation of the XML <Navigation></Navigation> node.
        ///// </summary>
        //[XmlElement("Navigation"), Browsable(false)]
        //public NavigationArray _Navigation { get; set; }

        ///// <summary>
        ///// List all views items.
        ///// </summary>
        //[XmlElement("Views")]
        //public Views.Views Views { get; set; }

        #endregion

        #region Properties

        ///// <summary>
        ///// List all entities, later used to construct the menus and views.
        ///// </summary>
        //[XmlIgnore]
        //public Entity[] EntityTypes { get { return _EntityTypes?.Entities; } }

        ///// <summary>
        ///// List all models.
        ///// </summary>
        //[XmlIgnore]
        //public Model[] Models { get { return _Models?.Models; } }

        ///// <summary>
        ///// List all entities, later used to construct the menus and views.
        ///// </summary>s
        //[XmlIgnore]
        //public Xml.Enums.Enum[] Enums { get { return _Enums?.Enums; } }

        /// <summary>
        /// Static texts used across the application.
        /// </summary>
        [XmlIgnore]
        public StaticText[] StaticTexts { get { return _StaticTexts?.Texts; } }


        /// <summary>
        /// List all available languages, will use the languages listed to show the language selection box.
        /// </summary>
        [XmlIgnore]
        public Language[] Languages { get { return _Languages?.Languages; } }

        ///// <summary>
        ///// List all menu items.
        ///// </summary>
        //[XmlIgnore]
        //public Menu[] Navigation { get { return _Navigation?.Menus; } }

        #endregion

        #region Methods

        ///// <summary>
        ///// Search all entities using the Id.
        ///// </summary>
        ///// <param name="Id">The entity Id.</param>
        ///// <returns>Return the Entity object, null if not found.</returns>
        //public Entity FindEntityById(string Id)
        //{
        //    return Array.Find(EntityTypes, x => x.Id == Id);
        //}

        ///// <summary>
        ///// Search all entities using the entity type.
        ///// </summary>
        ///// <param name="entityType">The entity type.</param>
        ///// <returns>Return the Entity object, null if not found.</returns>
        //public Entity FindEntity(Type entityType)
        //{
        //    if (entityType != null)
        //        return Array.Find(EntityTypes, x => x.Id == entityType.Name || x.TypeClr == entityType.FullName);
        //    return null; 
        //}

        ///// <summary>
        ///// Search all models using the model type name.
        ///// </summary>
        ///// <param name="modelType">The type of the model.</param>
        ///// <returns>Return the Model object, null if not found.</returns>
        //public Model FindModel(Type modelType)
        //{
        //    if (Models != null)
        //        return Array.Find(Models, x => x.Id == modelType.Name);
        //    return null;
        //}

        ///// <summary>
        ///// Search all ENUMs using the Id.
        ///// </summary>
        ///// <param name="Id">The ENUM Id.</param>
        ///// <returns>Return the ENUM object, null if not found.</returns>
        //public Enums.Enum FindEnum(string Id)
        //{
        //    if (Enums != null)
        //        return Array.Find(Enums, x => x.Id == Id);
        //    return null;
        //}


        ///// <summary>
        ///// Search all languages using the Code property.
        ///// </summary>
        ///// <param name="languageCode">The language code to search.</param>
        ///// <returns>Return the Language object, null if not found.</returns>
        //public Language FindLanguage(string languageCode)
        //{
        //    if ((Languages?.Length ?? 0) > 0)
        //        return Array.Find(Languages, l => l.Code == languageCode);
        //    return null;
        //}


        ///// <summary>
        ///// Search all Static Texts using the Key property.
        ///// </summary>
        ///// <param name="key">The static text key to search.</param>
        ///// <returns>Return the Static Text value string, string.Empty if not found.</returns>
        //public string GetStaticText(string key)
        //{
        //    if (StaticTexts != null && StaticTexts != null && StaticTexts.Length > 0)
        //    {
        //        var returnStr = Array.Find(StaticTexts, x => x.Key == key);
        //        if (!string.IsNullOrWhiteSpace(returnStr?.Value ?? string.Empty))
        //        {
        //            return Regex.Replace(returnStr.Value, @"\[(.*?)\]", match => { return match.ToString().Replace("[", "<").Replace("]", ">"); });
        //        }
        //    }
        //    return string.Empty;
        //}


        ///// <summary>
        ///// Search all DetailViews using the Id property.
        ///// </summary>
        ///// <param name="viewId">The id of the view to search.</param>
        ///// <returns>Return the DetailView object, null if not found.</returns>
        //public DetailView FindDetailView(string viewId)
        //{
        //    if ((Views?.DetailViews?.Length ?? 0) > 0)
        //    {
        //        return Array.Find(Views.DetailViews, view => view.ViewId == viewId);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Search all ListViews using the Id property.
        ///// </summary>
        ///// <param name="viewId">The id of the view to search.</param>
        ///// <returns>Return the ListView object, null if not found.</returns>
        //public ListView FindListView(string viewId)
        //{
        //    if ((Views?.ListViews?.Length ?? 0) > 0)
        //        return Array.Find(Views.ListViews, view => view.ViewId == viewId);
        //    return null;
        //}

        ///// <summary>
        ///// Search all LookUpViews using the Id property.
        ///// </summary>
        ///// <param name="viewId">The id of the view to search.</param>
        ///// <returns>Return the LookUpView object, null if not found.</returns>
        //public LookupView FindLookupView(string viewId)
        //{
        //    if ((Views?.LookupViews?.Length ?? 0) > 0)
        //        return Array.Find(Views.LookupViews, view => view.ViewId == viewId);
        //    return null;
        //}

        #endregion
    }
}