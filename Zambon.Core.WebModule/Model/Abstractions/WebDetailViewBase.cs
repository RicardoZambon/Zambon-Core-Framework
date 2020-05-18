using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebDetailViewBase<TButtonsParent, TButton> : DetailViewBase<TButtonsParent, TButton>, IWebDetailView<TButtonsParent, TButton>
        where TButtonsParent : WebButtonsParentBase<TButton>
            where TButton : WebButtonBase
    {
        #region XML Attributes

        [XmlAttribute]
        public string ControllerName { get; set; }

        [XmlAttribute]
        public string ActionName { get; set; }

        [XmlAttribute]
        public string ViewFolder { get; set; }

        [XmlAttribute]
        public string DefaultView { get; set; }

        #endregion
    }
}