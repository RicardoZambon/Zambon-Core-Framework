using System.Xml.Serialization;
using Zambon.Core.Module.Model.Nodes.Views;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Nodes.Views
{
    public sealed class WebDetailView<TButton> : DetailView<TButton>, IWebDetailView<TButton> where TButton : class, IWebButton
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