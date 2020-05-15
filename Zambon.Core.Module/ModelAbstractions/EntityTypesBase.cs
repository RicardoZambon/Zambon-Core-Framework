using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.ModelAbstractions
{
    public abstract class EntityTypesBase<TEntity> : BaseNode, IEntityTypes<TEntity> where TEntity : EntityBase
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