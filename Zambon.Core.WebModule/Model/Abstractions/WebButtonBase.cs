using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebButtonBase : ButtonBase, IWebButton
    {
        #region XML Attributes

        [XmlAttribute]
        public string CssClass { get; set; }

        [XmlAttribute]
        public string ControllerName { get; set; }

        [XmlAttribute]
        public string ActionName { get; set; }

        [XmlAttribute]
        public string ActionParameters { get; set; }

        [XmlAttribute]
        public string ActionMethod { get; set; }

        [XmlAttribute(nameof(UseFormPost)), Browsable(false)]
        public string BoolUseFormPost
        {
            get { return UseFormPost?.ToString(); }
            set { if (value != null) { bool.TryParse(value, out bool useFormPost); UseFormPost = useFormPost; } }
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public bool? UseFormPost { get; set; }

        #endregion
    }
}