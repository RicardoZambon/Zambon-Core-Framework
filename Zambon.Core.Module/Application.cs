using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Entities;
using Zambon.Core.Module.Serialization;

namespace Zambon.Core.Module
{
    /// <summary>
    /// Represent the root XML node.
    /// </summary>
    [XmlRoot]
    public class Application : BaseNode
    {
        #region XML Attributes

        #endregion

        #region XML Elements

        /// <summary>
        /// Represents a list of entity types in XML model file.
        /// </summary>
        [XmlElement(nameof(EntityTypes)), Browsable(false)]
        public EntityTypes _EntityTypes { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Represents a list of entity types in XML model file.
        /// </summary>
        public ChildItemCollection<Entity> Entities => _EntityTypes?.EntitiesList ?? new ChildItemCollection<Entity>(null);

        #endregion
    }
}