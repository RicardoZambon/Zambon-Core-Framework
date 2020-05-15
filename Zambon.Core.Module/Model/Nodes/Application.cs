using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Entities;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;

namespace Zambon.Core.Module.Model.Nodes
{
    /// <summary>
    /// Represent the root XML node.
    /// </summary>
    [XmlRoot]
    public class Application : ApplicationBase<EntityTypes, Entity, Properties, Property>
    {
    }
}