using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    /// <summary>
    /// Represents a node <TitleDefaults /> from XML Application Model. Define default values for the application title.
    /// </summary>
    public class TitleDefaults : XmlNode
    {
        #region XML Attributes

        /// <summary>
        /// The ShowEnvironment attribute from XML. Indicates if the environment title should appears at the beginning of the application title.
        /// </summary>
        [XmlAttribute("ShowEnvironment"), Browsable(false)]
        public string BoolShowEnvironment
        {
            get { return ShowEnvironment?.ToString(); }
            set { if (value != null) { bool.TryParse(value, out bool canEdit); ShowEnvironment = canEdit; } }
        }

        /// <summary>
        /// The ShowVersion attribute from XML. Indicates if the version should appears at the ending of the application title.
        /// </summary>
        [XmlAttribute("ShowVersion"), Browsable(false)]
        public string BoolShowVersion
        {
            get { return ShowVersion?.ToString(); }
            set { if (value != null) { bool.TryParse(value, out bool canEdit); ShowVersion = canEdit; } }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The CanEdit attribute from XML. Indicates if the list views should be editable or not.
        /// </summary>
        [XmlIgnore]
        public bool? ShowEnvironment { get; set; }

        /// <summary>
        /// The ShowPagination attribute from XML. Indicates if should display or not a pagination.
        /// </summary>
        [XmlIgnore]
        public bool? ShowVersion { get; set; }

        #endregion
    }
}