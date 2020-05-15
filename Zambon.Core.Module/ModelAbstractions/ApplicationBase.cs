using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.ModelAbstractions
{
    public abstract class ApplicationBase
        <TEntityTypes, TEntity> : BaseNode
        where TEntityTypes : EntityTypesBase<TEntity>, IEntityTypes<TEntity>
            where TEntity : EntityBase, IEntity
    {
        #region Constants

        private const string ENTITY_TYPES = "EntityTypes";

        #endregion

        #region XML Elements

        private TEntityTypes _entityTypes;
        [XmlElement(ENTITY_TYPES), Browsable(false)]
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