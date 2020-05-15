using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;

namespace Zambon.Core.Module.Model.Nodes.Entities
{
    /// <summary>
    /// Represents entities listed under EntityTypes in XML model.
    /// </summary>
    public class Entity : EntityBase<Properties.Properties, Property>
    {
    }
}