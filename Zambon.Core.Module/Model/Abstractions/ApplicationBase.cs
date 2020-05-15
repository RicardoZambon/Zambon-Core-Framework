using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ApplicationBase<TEntityTypes, TEntity, TProperties, TProperty> : BaseNode, IApplication<TEntityTypes, TEntity, TProperties, TProperty>
        where TEntityTypes : IEntityTypes<TEntity, TProperties, TProperty>
            where TEntity : IEntity<TProperties, TProperty>
                where TProperties : IProperties<TProperty>
                    where TProperty : IProperty
    {
        #region Constants

        protected const string APPLICATION_NODE = "Application";

        private const string ENTITY_TYPES_NODE = "EntityTypes";

        #endregion

        #region XML Elements

        private TEntityTypes _entityTypes;
        [XmlElement(ENTITY_TYPES_NODE), Browsable(false)]
        public TEntityTypes _EntityTypes
        {
            get => _entityTypes;
            set => SetParent(value, ref _entityTypes);
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public ChildItemCollection<TEntity> Entities => _EntityTypes?.EntitiesList ?? new ChildItemCollection<TEntity>(null);

        #endregion
    }
}