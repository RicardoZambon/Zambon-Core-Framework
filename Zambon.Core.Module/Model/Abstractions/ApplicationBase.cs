using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ApplicationBase<TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews> : SerializeNodeBase, IApplication<TEntity, TEnum, TStaticText, TLanguage, TModuleConfigurations, TMenu, TViews>
        where TEntity : class, IEntity
        where TEnum : class, IEnum
        where TStaticText : class, IStaticText
        where TLanguage : class, ILanguage
        where TModuleConfigurations : class, IModuleConfigurations
        where TMenu : class, IMenu<TMenu>
        where TViews : class, IViews
    {
        #region Constants

        protected const string APPLICATION_NODE = "Application";

        #endregion

        #region XML Arrays

        [XmlArray]
        public ChildItemCollection<TEntity> EntityTypes { get; set; }

        [XmlArray]
        public ChildItemCollection<TEnum> Enums { get; set; }

        [XmlArray]
        public ChildItemCollection<TStaticText> StaticTexts { get; set; }

        [XmlArray]
        public ChildItemCollection<TLanguage> Languages { get; set; }

        [XmlArray]
        public ChildItemCollection<TMenu> Menus { get; set; }

        #endregion

        #region XML Elements

        private TModuleConfigurations moduleConfigurations;
        [XmlElement]
        public TModuleConfigurations ModuleConfigurations
        {
            get => moduleConfigurations;
            set => SetParent(value, ref moduleConfigurations);
        }

        private TViews views;
        [XmlElement]
        public TViews Views
        {
            get => views;
            set => SetParent(value, ref views);
        }

        #endregion

        #region Constructors

        public ApplicationBase()
        {
            EntityTypes = new ChildItemCollection<TEntity>(this);
            Enums = new ChildItemCollection<TEnum>(this);
            StaticTexts = new ChildItemCollection<TStaticText>(this);
            Languages = new ChildItemCollection<TLanguage>(this);
            Menus = new ChildItemCollection<TMenu>(this);
        }

        #endregion
    }
}