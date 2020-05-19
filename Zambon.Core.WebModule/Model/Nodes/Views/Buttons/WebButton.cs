using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Model.Nodes.Views.Buttons;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Nodes.Views.Buttons
{
    public abstract class WebButton<TSubButton> : Button<TSubButton>, IWebButton<TSubButton>
        where TSubButton : class, IWebButton<TSubButton>
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

    public sealed class WebButton : WebButton<WebButton>
    {

    }
}