using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    /// <summary>
    /// Represents a node <PaintOption /> from XML Application Model.
    /// </summary>
    public class PaintOption : XmlNode, ICondition
    {

        /// <summary>
        /// The Id attribute from XML. The identification of this rule used when merging.
        /// </summary>
        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        /// <summary>
        /// The BackColor attribute from XML. The back color name to paint the row when the Condition is true.
        /// </summary>
        [XmlAttribute("BackColor")]
        public string BackColor { get; set; }

        /// <summary>
        /// The ForeColor attribute from XML. The fore color name to paint the row when the Condition is true.
        /// </summary>
        [XmlAttribute("ForeColor")]
        public string ForeColor { get; set; }

        /// <summary>
        /// The CssClass attribute from XML. The css class name to use in row when the Condition is true.
        /// </summary>
        [XmlAttribute("CssClass")]
        public string CssClass { get; set; }


        /// <summary>
        /// The Condition attribute from XML. Condition to determine when use or not this rule.
        /// </summary>
        [XmlAttribute("Condition")]
        public string Condition { get; set; }

        /// <summary>
        /// The ConditionArguments attribute from XML. Condition arguments to determine when use or not this rule, should be separated by ",".
        /// </summary>
        [XmlAttribute("ConditionArguments")]
        public string ConditionArguments { get; set; }

    }
}