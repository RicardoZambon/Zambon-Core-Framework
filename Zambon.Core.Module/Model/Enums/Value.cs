using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public class Value : BaseNode
    {
        #region XML Attributes

        /// <summary>
        /// The Key attribute from XML. The int value from ENUM type.
        /// </summary>
        [XmlAttribute, MergeKey]
        public string Key { get; set; }

        /// <summary>
        /// The DisplayName attribute from XML. The display name should be used in application, by default will use the Display attribute.
        /// </summary>
        [XmlAttribute]
        public string DisplayName { get; set; }

        #endregion
    }
}