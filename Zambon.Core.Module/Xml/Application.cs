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
    [XmlRoot]
    public class Application : XmlNode
    {

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Fullname")]
        public string FullName { get; set; }

        [XmlAttribute("MenuName")]
        public string MenuName { get; set; }


        [XmlIgnore]
        public Entity[] EntityTypes { get { return _EntityTypes?.Entities; } }

        [XmlIgnore]
        public Language[] Languages { get { return _Languages?.Languages; } }

        [XmlIgnore]
        public StaticText[] StaticTexts { get { return _StaticTexts?.Texts; } }

        [XmlElement("ModuleConfiguration")]
        public ModuleConfiguration ModuleConfiguration { get; set; }

        [XmlIgnore]
        public Menu[] Navigation { get { return _Navigation?.Menus; } }

        [XmlElement("Views")]
        public Views.Views Views { get; set; }


        [XmlElement("EntityTypes"), Browsable(false)]
        public EntityTypesArray _EntityTypes { get; set; }

        [XmlElement("Languages"), Browsable(false)]
        public LanguagesArray _Languages { get; set; }

        [XmlElement("StaticTexts"), Browsable(false)]
        public StaticTextsArray _StaticTexts { get; set; }

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

        public Entity FindEntityById(string Id)
        {
            return Array.Find(EntityTypes, x => x.Id == Id);
        }

        public Entity FindEntityByClrType(string typeClr)
        {
            return Array.Find(EntityTypes, x => x.TypeClr == typeClr);
        }


        public Language FindLanguage(string languageCode)
        {
            if ((Languages?.Length ?? 0) > 0)
                return Array.Find(Languages, l => l.Code == languageCode);
            return null;
        }


        public string GetStaticText(string _key)
        {
            if (StaticTexts != null && StaticTexts != null && StaticTexts.Length > 0)
            {
                var returnStr = Array.Find(StaticTexts, x => x.Key == _key);
                if (returnStr != null && !string.IsNullOrWhiteSpace(returnStr.Value))
                    return Regex.Replace(returnStr.Value, @"\[(.*?)\]", match => { return match.ToString().Replace("[", "<").Replace("]", ">"); });
            }
            return string.Empty;
        }


        public DetailView FindDetailView(string Id)
        {
            if ((Views?.DetailViews?.Length ?? 0) > 0)
                return Array.Find(Views.DetailViews, view => view.ViewId == Id);
            return null;
        }

        public ListView FindListView(string Id)
        {
            if ((Views?.ListViews?.Length ?? 0) > 0)
                return Array.Find(Views.ListViews, view => view.ViewId == Id);
            return null;
        }

        public LookupView FindLookupView(string Id)
        {
            if ((Views?.LookupViews?.Length ?? 0) > 0)
                return Array.Find(Views.LookupViews, view => view.ViewId == Id);
            return null;
        }

        #endregion

    }
}