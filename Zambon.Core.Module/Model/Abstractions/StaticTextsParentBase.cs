using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class StaticTextsParentBase<TStaticText> : SerializeNodeBase, IStaticTextsParent<TStaticText>
        where TStaticText : IStaticText
    {
        #region Constants

        private const string STATIC_TEXTS_NODE = "StaticText";

        #endregion

        #region XML Attributes

        [XmlElement(STATIC_TEXTS_NODE)]
        public ChildItemCollection<TStaticText> StaticTextsList { get; set; }

        #endregion

        #region Constructors

        public StaticTextsParentBase()
        {
            StaticTextsList = new ChildItemCollection<TStaticText>(this);
        }

        #endregion
    }
}