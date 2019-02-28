using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public abstract class BaseModal : BaseSubView
    {

        [XmlAttribute("Title")]
        public string Title { get; set; }

    }
}