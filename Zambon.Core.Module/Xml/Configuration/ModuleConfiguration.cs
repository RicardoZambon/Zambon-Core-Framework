using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    /// <summary>
    /// Represents a node <ModuleConfiguration></ModuleConfiguration> from XML Application Model. Define default values for all application.
    /// </summary>
    public class ModuleConfiguration : XmlNode
    {
        #region XML Attributes

        /// <summary>
        /// Represents an element <TitleDefaults /> from XML Application Model. Define default values for the application title.
        /// </summary>
        [XmlElement("TitleDefaults")]
        public TitleDefaults TitleDefaults { get; set; }

        #endregion
    }
}