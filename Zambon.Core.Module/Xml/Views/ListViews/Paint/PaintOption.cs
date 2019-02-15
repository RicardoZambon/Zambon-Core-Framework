using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    public class PaintOption : XmlNode, IExpression
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("BackColor")]
        public string BackColor { get; set; }

        [XmlAttribute("ForeColor")]
        public string ForeColor { get; set; }

        [XmlAttribute("CustomClass")]
        public string CustomClass { get; set; }

        [XmlAttribute("ConditionExpression")]
        public string ConditionExpression { get; set; }

        [XmlAttribute("ConditionArguments")]
        public string ConditionArguments { get; set; }

        [XmlIgnore]
        public string[] ConditionArgumentsList
        {
            get { return (ConditionArguments != null ? ConditionArguments.Split(',') : new string[] { }); }
        }

    }
}