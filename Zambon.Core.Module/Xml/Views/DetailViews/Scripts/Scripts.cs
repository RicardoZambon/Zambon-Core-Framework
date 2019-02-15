using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.DetailViews.Scripts
{
    public class Scripts : XmlNode
    {

        [XmlElement("Script")]
        public Script[] Script { get; set; }

    }
}