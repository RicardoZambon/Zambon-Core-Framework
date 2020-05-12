using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Entities.Properties;
using Zambon.Core.Module.Serialization;

namespace Zambon.Core.Module.Entities
{
    /// <summary>
    /// Represents entities listed under EntityTypes in XML model.
    /// </summary>
    public class Entity : BaseNode
    {
        #region XML Attributes

        /// <summary>
        /// The Id of the entity type. This property is used to merge same entities across different models.
        /// </summary>
        [XmlAttribute, Merge]
        public string Id { get; set; }

        /// <summary>
        /// The name of the entity type.
        /// </summary>
        [XmlAttribute]
        public string DisplayName { get; set; }

        /// <summary>
        /// Singular name of each entity type.
        /// </summary>
        [XmlAttribute]
        public string SingularName { get; set; }

        /// <summary>
        /// Icon for this entity display at specific places.
        /// </summary>
        [XmlAttribute]
        public string Icon { get; set; }

        /// <summary>
        /// Optional property. Only use this is this entity should always be returned using a custom query from database.
        /// </summary>
        [XmlAttribute]
        public string FromSql { get; set; }

        /// <summary>
        /// If the FromSql require parameters to pass to SQL execute when returning the objects.
        /// </summary>
        [XmlAttribute]
        public string FromSqlParameters { get; set; }

        /// <summary>
        /// The complete CLR type of the entity type.
        /// </summary>
        [XmlAttribute]
        public string TypeClr { get; set; }

        #endregion

        #region XML Elements

        /// <summary>
        /// List of all properties available in XML file.
        /// </summary>
        [XmlElement(nameof(Entities.Properties.Properties)), Browsable(false)]
        public Properties.Properties _Properties { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// List of all properties available in XML file.
        /// </summary>
        [XmlIgnore]
        public ChildItemCollection<Property> Properties => _Properties?.PropertiesList ?? new ChildItemCollection<Property>(null);


        #endregion
    }
}