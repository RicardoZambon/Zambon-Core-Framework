using System.Xml.Serialization;
using Zambon.Core.Module.Serialization;

namespace Zambon.Core.Module.Enums
{
    /// <summary>
    /// Represents a list of enums used in application.
    /// </summary>
    public class Enums : BaseNode
    {
        #region XML Elements

        /// <summary>
        /// List of all enums.
        /// </summary>
        [XmlElement(nameof(Enum))]
        public ChildItemCollection<Enum> EnumsList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Enums()
        {
            EnumsList = new ChildItemCollection<Enum>(this);
        }

        #endregion
    }
}