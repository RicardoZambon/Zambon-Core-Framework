using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public class EnumBase<TValue> : SerializeNodeBase, IEnum<TValue>
        where TValue : ValueBase
    {
        #region Constants

        private const string VALUES_NODE = "Value";

        #endregion

        #region XML Attributes

        [XmlAttribute]
        public string Id { get; set; }

        #endregion

        #region XML Elements

        [XmlElement(VALUES_NODE)]
        public ChildItemCollection<TValue> ValuesList { get; set; }

        #endregion

        #region Constructors

        public EnumBase()
        {
            ValuesList = new ChildItemCollection<TValue>(this);
        }

        #endregion
    }
}