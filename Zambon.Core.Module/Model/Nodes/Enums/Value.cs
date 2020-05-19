using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Enums
{
    public class Value : SerializeNodeBase, IValue
    {
        #region XML Attributes

        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        #endregion

        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}