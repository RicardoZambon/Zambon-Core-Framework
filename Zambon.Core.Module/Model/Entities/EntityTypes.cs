using System.Xml;
using System.Xml.Serialization;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Entities
{
    /// <summary>
    /// Represents a list of entity types in XML model file.
    /// </summary>
    public class EntityTypes : BaseNode
    {
        #region XML Attributes

        /// <summary>
        /// List of all entities available in XML model.
        /// </summary>
        [XmlElement(nameof(Entity))]
        public ChildItemCollection<Entity> EntitiesList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EntityTypes()
        {
            EntitiesList = new ChildItemCollection<Entity>(this);
        }

        #endregion
    }
}