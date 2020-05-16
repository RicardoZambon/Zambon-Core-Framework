using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class EnumsParentBase<TEnum, TValue> : SerializeNodeBase, IEnumsParent<TEnum, TValue>
        where TEnum : EnumBase<TValue>
            where TValue : ValueBase
    {
        #region Constants

        private const string ENUMS_NODE = "Enum";

        #endregion

        #region XML Elements

        [XmlElement(ENUMS_NODE)]
        public ChildItemCollection<TEnum> EnumsList { get; set; }

        #endregion

        #region Constructors

        public EnumsParentBase()
        {
            EnumsList = new ChildItemCollection<TEnum>(this);
        }

        #endregion
    }
}