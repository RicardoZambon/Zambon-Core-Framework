using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class EntityTypesParentBase<TEntity, TPropertiesParent, TProperty> : SerializeNodeBase, IEntityTypesParent<TEntity, TPropertiesParent, TProperty>
        where TEntity : EntityBase<TPropertiesParent, TProperty>
            where TPropertiesParent : PropertiesParentBase<TProperty>
                where TProperty : PropertyBase
    {
        #region Constants

        private const string ENTITIES_NODE = "Entity";

        #endregion

        #region XML Attributes

        [XmlElement(ENTITIES_NODE)]
        public ChildItemCollection<TEntity> EntitiesList { get; set; }

        #endregion

        #region Constructors

        public EntityTypesParentBase()
        {
            EntitiesList = new ChildItemCollection<TEntity>(this);
        }

        #endregion
    }
}