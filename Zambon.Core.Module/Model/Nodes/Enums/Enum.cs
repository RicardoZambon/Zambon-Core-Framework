using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Enums
{
    public class Enum<TValue> : SerializeNodeBase, IEnum<TValue>
        where TValue : class, IValue
    {
        #region XML Attributes

        [XmlAttribute]
        public string Id { get; set; }

        #endregion

        #region XML Arrays

        [XmlArray]
        public ChildItemCollection<TValue> Values { get; set; }

        #endregion

        #region Constructors

        public Enum()
        {
            Values = new ChildItemCollection<TValue>(this);
        }

        #endregion

        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}