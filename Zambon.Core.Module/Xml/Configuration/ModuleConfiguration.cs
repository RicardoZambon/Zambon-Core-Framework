using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    /// <summary>
    /// Represents a node <ModuleConfiguration></ModuleConfiguration> from XML Application Model. Define default values for all application.
    /// </summary>
    public class ModuleConfiguration : XmlNode
    {

        /// <summary>
        /// Represents an element <LoginDefaults /> from XML Application Model. Define default values for login page.
        /// </summary>
        [XmlElement("LoginDefaults")]
        public LoginDefaults LoginDefaults { get; set; }

        /// <summary>
        /// Represents an element <DetailViewDefaults /> from XML Application Model. Define default values for all DetailViews.
        /// </summary>
        [XmlElement("DetailViewDefaults")]
        public DetailViewDefaults DetailViewDefaults { get; set; }

        /// <summary>
        /// Represents an element <ListViewDefaults /> from XML Application Model. Define default values for all ListViews.
        /// </summary>
        [XmlElement("ListViewDefaults")]
        public ListViewDefaults ListViewDefaults { get; set; }

    }
}