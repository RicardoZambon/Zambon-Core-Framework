using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEntityTypes<TEntity, TProperties, TProperty> : IParent
        where TEntity : IEntity<TProperties, TProperty>
        where TProperties : IProperties<TProperty>
        where TProperty : IProperty
    {
        ChildItemCollection<TEntity> EntitiesList { get; set; }
    }
}