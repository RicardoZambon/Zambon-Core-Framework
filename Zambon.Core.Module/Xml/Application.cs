using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Xml.Configuration;
using Zambon.Core.Module.Xml.EntityTypes;
using Zambon.Core.Module.Xml.Languages;
using Zambon.Core.Module.Xml.Navigation;
using Zambon.Core.Module.Xml.StaticTexts;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.Module.Xml
{
    /// <summary>
    /// Represents the root class from XML.
    /// </summary>
    [XmlRoot]
    public class Application : XmlNode
    {

        /// <summary>
        /// Application name.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Long name for application
        /// </summary>
        [XmlAttribute("Fullname")]
        public string FullName { get; set; }

        /// <summary>
        /// Name to display in menu, if empty will use the Name property.
        /// </summary>
        [XmlAttribute("MenuName")]
        public string MenuName { get; set; }


        /// <summary>
        /// List all entities, later used to construct the menus and views.
        /// </summary>
        [XmlIgnore]
        public Entity[] EntityTypes { get { return _EntityTypes?.Entities; } }

        /// <summary>
        /// List all available languages, will use the languages listed to show the language selection box.
        /// </summary>
        [XmlIgnore]
        public Language[] Languages { get { return _Languages?.Languages; } }

        /// <summary>
        /// Static texts used across the application.
        /// </summary>
        [XmlIgnore]
        public StaticText[] StaticTexts { get { return _StaticTexts?.Texts; } }

        /// <summary>
        /// Module default configuration.
        /// </summary>
        [XmlElement("ModuleConfiguration")]
        public ModuleConfiguration ModuleConfiguration { get; set; }

        /// <summary>
        /// List all menu items.
        /// </summary>
        [XmlIgnore]
        public Menu[] Navigation { get { return _Navigation?.Menus; } }

        /// <summary>
        /// List all views items.
        /// </summary>
        [XmlElement("Views")]
        public Views.Views Views { get; set; }


        /// <summary>
        /// Element representation of the Xml <EntityTipes></EntityTipes> node.
        /// </summary>
        [XmlElement("EntityTypes"), Browsable(false)]
        public EntityTypesArray _EntityTypes { get; set; }

        /// <summary>
        /// Element representation of the Xml <Languages></Languages> node.
        /// </summary>
        [XmlElement("Languages"), Browsable(false)]
        public LanguagesArray _Languages { get; set; }

        /// <summary>
        /// Element representation of the Xml <StaticTexts></StaticTexts> node.
        /// </summary>
        [XmlElement("StaticTexts"), Browsable(false)]
        public StaticTextsArray _StaticTexts { get; set; }

        /// <summary>
        /// Element representation of the Xml <Navigation></Navigation> node.
        /// </summary>
        [XmlElement("Navigation"), Browsable(false)]
        public NavigationArray _Navigation { get; set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

            if (string.IsNullOrWhiteSpace(MenuName))
                MenuName = Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Search all entities using the Id.
        /// </summary>
        /// <param name="Id">The entity Id.</param>
        /// <returns>Return the Entity object, null if not found.</returns>
        public Entity FindEntityById(string Id)
        {
            return Array.Find(EntityTypes, x => x.Id == Id);
        }

        /// <summary>
        /// Search all entities using the CLR Type.
        /// </summary>
        /// <param name="typeClr">The entity CLR Type.</param>
        /// <returns>Return the Entity object, null if not found.</returns>
        public Entity FindEntityByClrType(string typeClr)
        {
            return Array.Find(EntityTypes, x => x.TypeClr == typeClr);
        }

        /// <summary>
        /// Search all languages using the Code property.
        /// </summary>
        /// <param name="languageCode">The language code to search.</param>
        /// <returns>Return the Language object, null if not found.</returns>
        public Language FindLanguage(string languageCode)
        {
            if ((Languages?.Length ?? 0) > 0)
                return Array.Find(Languages, l => l.Code == languageCode);
            return null;
        }


        /// <summary>
        /// Search all Static Texts using the Key property.
        /// </summary>
        /// <param name="key">The static text key to search.</param>
        /// <returns>Return the Static Text value string, string.Empty if not found.</returns>
        public string GetStaticText(string key)
        {
            if (StaticTexts != null && StaticTexts != null && StaticTexts.Length > 0)
            {
                var returnStr = Array.Find(StaticTexts, x => x.Key == key);
                if (returnStr != null && !string.IsNullOrWhiteSpace(returnStr.Value))
                    return Regex.Replace(returnStr.Value, @"\[(.*?)\]", match => { return match.ToString().Replace("[", "<").Replace("]", ">"); });
            }
            return string.Empty;
        }


        /// <summary>
        /// Search all DetailViews using the Id property.
        /// </summary>
        /// <param name="viewId">The id of the view to search.</param>
        /// <returns>Return the DetailView object, null if not found.</returns>
        public DetailView FindDetailView(string viewId)
        {
            if ((Views?.DetailViews?.Length ?? 0) > 0)
                return Array.Find(Views.DetailViews, view => view.ViewId == viewId);
            return null;
        }

        /// <summary>
        /// Search all ListViews using the Id property.
        /// </summary>
        /// <param name="viewId">The id of the view to search.</param>
        /// <returns>Return the ListView object, null if not found.</returns>
        public ListView FindListView(string viewId)
        {
            if ((Views?.ListViews?.Length ?? 0) > 0)
                return Array.Find(Views.ListViews, view => view.ViewId == viewId);
            return null;
        }

        /// <summary>
        /// Search all LookUpViews using the Id property.
        /// </summary>
        /// <param name="viewId">The id of the view to search.</param>
        /// <returns>Return the LookUpView object, null if not found.</returns>
        public LookupView FindLookupView(string viewId)
        {
            if ((Views?.LookupViews?.Length ?? 0) > 0)
                return Array.Find(Views.LookupViews, view => view.ViewId == viewId);
            return null;
        }

        #endregion

    }
}