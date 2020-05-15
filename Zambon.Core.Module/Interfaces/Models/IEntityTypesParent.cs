using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEntityTypesParent<TEntity, TPropertiesParent, TProperty> : IParent
        where TEntity : IEntity<TPropertiesParent, TProperty>
        where TPropertiesParent : IPropertiesParent<TProperty>
        where TProperty : IProperty
    {
        ChildItemCollection<TEntity> EntitiesList { get; set; }
    }
}