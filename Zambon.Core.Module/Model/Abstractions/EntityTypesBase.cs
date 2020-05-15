using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class EntityTypesBase<TEntity, TProperties, TProperty> : BaseNode, IEntityTypes<TEntity, TProperties, TProperty>
        where TEntity : IEntity<TProperties, TProperty>
            where TProperties : IProperties<TProperty>
                where TProperty : IProperty
    {
        #region Constants

        private const string ENTITIES = "Entity";

        #endregion

        #region XML Attributes

        [XmlElement(ENTITIES)]
        public ChildItemCollection<TEntity> EntitiesList { get; set; }

        #endregion

        #region Constructors

        public EntityTypesBase()
        {
            EntitiesList = new ChildItemCollection<TEntity>(this);
        }

        #endregion
    }
}