using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    /// <summary>
    /// Define default configuration values for all DetailViews.
    /// </summary>
    public class DetailViewDefaults : XmlNode
    {

        /// <summary>
        /// Define the default action name the DetailView should post when submitting the form.
        /// </summary>
        [XmlAttribute("DefaultAction")]
        public string DefaultAction { get; set; }

        /// <summary>
        /// Define the default name of the .cshtml file.
        /// </summary>
        [XmlAttribute("DefaultView")]
        public string DefaultView { get; set; }        

    }
}