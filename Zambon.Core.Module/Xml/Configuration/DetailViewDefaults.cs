using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    /// <summary>
    /// Represents a node <DetailViewDefaults /> from XML Application Model. Define default values for all DetailViews.
    /// </summary>
    public class DetailViewDefaults : XmlNode
    {
        /// <summary>
        /// The DefaultAction attribute from XML. The default action name the DetailView should post when submitting the form.
        /// </summary>
        [XmlAttribute("DefaultAction")]
        public string DefaultAction { get; set; }

        /// <summary>
        /// The DefaultView attribute from XML. The default name of the .cshtml file.
        /// </summary>
        [XmlAttribute("DefaultView")]
        public string DefaultView { get; set; }        
    }
}