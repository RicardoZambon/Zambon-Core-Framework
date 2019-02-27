using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    public class PaintOption : XmlNode, ICondition
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("BackColor")]
        public string BackColor { get; set; }

        [XmlAttribute("ForeColor")]
        public string ForeColor { get; set; }

        [XmlAttribute("CssClass")]
        public string CssClass { get; set; }


        [XmlAttribute("ConditionExpression")]
        public string Condition { get; set; }

        [XmlAttribute("ConditionArguments")]
        public string ConditionArguments { get; set; }

    }
}