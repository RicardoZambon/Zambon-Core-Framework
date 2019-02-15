using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public abstract class BaseModal : BaseSubView
    {

        [XmlAttribute("Level")]
        public int Level { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

    }
}