using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    /// <summary>
    /// Represents a node <LoginDefaults /> from XML Application Model. Define default values for login page.
    /// </summary>
    public class LoginDefaults : XmlNode
    {

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("Theme")]
        public string Theme { get; set; }

    }
}