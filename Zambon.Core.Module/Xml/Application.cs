using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml.Configuration;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

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


        [XmlElement("EntityTypes")]
        public EntityTypes.EntityTypes EntityTypes { get; set; }

        [XmlElement("StaticTexts")]
        public StaticTexts.StaticTexts StaticTexts { get; set; }

        [XmlElement("ModuleConfiguration")]
        public ModuleConfiguration ModuleConfiguration { get; set; }

        [XmlElement("Views")]
        public Views.Views Views { get; set; }

        [XmlElement("Navigation")]
        public Navigation.Navigation Navigation { get; set; }


        #region Overrides

        internal override void OnLoading(Application app, CoreDbContext ctx)
        {
            base.OnLoading(app, ctx);

            if (string.IsNullOrWhiteSpace(MenuName))
                MenuName = Name;
        }

        #endregion

        #region Methods

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


        public EntityTypes.Entity FindEntityById(string Id)
        {
            return EntityTypes.Entities.FirstOrDefault(x => x.Id == Id);
        }
        public EntityTypes.Entity FindEntityByClrType(string typeClr)
        {
            return EntityTypes.Entities.FirstOrDefault(x => x.TypeClr == typeClr);
        }

        public string GetStaticText(string _key)
        {
            if (StaticTexts != null && StaticTexts.Texts != null && StaticTexts.Texts.Length > 0)
            {
                var returnStr = Array.Find(StaticTexts.Texts, x => x.Key == _key);
                if (returnStr != null && !string.IsNullOrWhiteSpace(returnStr.Value))
                    return Regex.Replace(returnStr.Value, @"\[(.*?)\]", match => { return match.ToString().Replace("[", "<").Replace("]", ">"); });
            }
            return string.Empty;
        }

        #endregion

    }
}