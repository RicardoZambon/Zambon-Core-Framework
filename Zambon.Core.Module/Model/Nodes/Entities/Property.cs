using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Extensions;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.EntityTypes
{
    public class Property : SerializeNodeBase, IProperty
    {
        #region XML Attributes

        /// <summary>
        /// The property name.
        /// </summary>
        [XmlAttribute, MergeKey]
        public string Name { get; set; }

        /// <summary>
        /// The text when displaying this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute]
        public string DisplayName { get; set; }

        /// <summary>
        /// The text when displaying the input for this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute]
        public string Prompt { get; set; }

        /// <summary>
        /// The text when displaying the description for this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute]
        public string Description { get; set; }

        #endregion

        #region Constructors

        public Property()
        {

        }

        public Property(Microsoft.EntityFrameworkCore.Metadata.IProperty dbProperty) : this()
        {
            Name = dbProperty.Name;

            dbProperty.PropertyInfo.GetDisplay(out var displayName, out var prompt, out var description);

            DisplayName = displayName;
            Prompt = prompt;
            Description = description;
        }

        #endregion
    }
}