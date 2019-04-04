using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    /// <summary>
    /// Base class used for ModalViews.
    /// </summary>
    public abstract class BaseModal : BaseSubView
    {
        /// <summary>
        /// Text to show in the modal title.
        /// </summary>
        [XmlAttribute("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Current level of the modal, automatically calculated when generating the pages.
        /// </summary>
        [XmlIgnore]
        public int Level { get; set; }
    }
}