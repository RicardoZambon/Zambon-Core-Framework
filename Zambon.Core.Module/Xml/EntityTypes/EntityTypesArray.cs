using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    /// <summary>
    /// Represents a list of entity types used in CoreDbContext. IEntity or IQuery.
    /// </summary>
    public class EntityTypesArray : XmlNode
    {

        /// <summary>
        /// List of all entity types.
        /// </summary>
        [XmlElement("Entity")]
        public Entity[] Entities { get; set; }

    }
}