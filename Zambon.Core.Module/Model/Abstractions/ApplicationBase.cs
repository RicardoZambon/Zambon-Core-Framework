using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ApplicationBase<TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu> : SerializeNodeBase, IApplication<TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu>
        where TEntityTypesParent : IEntityTypesParent<TEntity, TPropertiesParent, TProperty>
            where TEntity : IEntity<TPropertiesParent, TProperty>
                where TPropertiesParent : IPropertiesParent<TProperty>
                    where TProperty : IProperty
        where TStaticTextsParent : IStaticTextsParent<TStaticText>
            where TStaticText : IStaticText
        where TNavigationParent : INavigationParent<TMenu>
            where TMenu : IMenu<TMenu>
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

        private TStaticTextsParent _staticTexts;
        [XmlElement(nameof(StaticTexts)), Browsable(false)]
        public TStaticTextsParent _StaticTexts
        {
            get => _staticTexts;
            set => SetParent(value, ref _staticTexts);
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
        public ChildItemCollection<TStaticText> StaticTexts => _StaticTexts?.StaticTextsList ?? new ChildItemCollection<TStaticText>(null);


        [XmlIgnore]
        public ChildItemCollection<TMenu> Menus => _Navigation?.MenusList ?? new ChildItemCollection<TMenu>(null);

        #endregion
    }
}