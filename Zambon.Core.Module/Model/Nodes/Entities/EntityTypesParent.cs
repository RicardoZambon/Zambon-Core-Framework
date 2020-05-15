using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;

namespace Zambon.Core.Module.Model.Nodes.Entities
{
    public class EntityTypesParent : EntityTypesParentBase<Entity, Properties.PropertiesParent, Property>
    {
        public EntityTypesParent() : base()
        {
        }
    }
}