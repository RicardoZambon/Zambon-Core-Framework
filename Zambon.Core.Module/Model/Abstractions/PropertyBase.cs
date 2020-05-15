﻿using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class PropertyBase : SerializeNodeBase, IProperty
    {
        #region XML Attributes

        /// <summary>
        /// The property name.
        /// </summary>
        [XmlAttribute("Name"), MergeKey]
        public string Name { get; set; }

        /// <summary>
        /// The text when displaying this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The text when displaying the input for this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute("Prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// The text when displaying the description for this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute("Description")]
        public string Description { get; set; }

        #endregion
    }
}