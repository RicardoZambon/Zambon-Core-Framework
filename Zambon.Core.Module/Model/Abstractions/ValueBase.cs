using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ValueBase : SerializeNodeBase, IValue
    {
        #region XML Attributes

        [XmlAttribute]
        public string Value { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        #endregion
    }
}