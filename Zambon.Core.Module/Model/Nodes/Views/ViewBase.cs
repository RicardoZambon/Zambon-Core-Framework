using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public abstract class ViewBase : SerializeNodeBase, IView
    {
        #region XML Attributes

        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string EntityId { get; set; }

        #endregion

        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}