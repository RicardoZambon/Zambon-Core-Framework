using System.Xml.Serialization;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Languages
{
    /// <summary>
    /// Represents a node <Languages></Languages> from XML Application Model.
    /// </summary>
    public class Languages : BaseNode
    {
        #region XML Elements

        /// <summary>
        /// List of <Language /> elements.
        /// </summary>
        [XmlElement(nameof(Language))]
        public ChildItemCollection<Language> LanguagesList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Languages()
        {
            LanguagesList = new ChildItemCollection<Language>(this);
        }

        #endregion
    }
}