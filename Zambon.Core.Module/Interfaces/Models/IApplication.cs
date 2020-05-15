using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IApplication<TEntityTypes, TEntity, TProperties, TProperty>
        where TEntityTypes : IEntityTypes<TEntity, TProperties, TProperty>
            where TEntity : IEntity<TProperties, TProperty>
                where TProperties : IProperties<TProperty>
                    where TProperty : IProperty
    {
        #region XML Elements

        TEntityTypes _EntityTypes { get; set; }

        #endregion

        #region Properties

        ChildItemCollection<TEntity> Entities { get; }

        #endregion
    }
}