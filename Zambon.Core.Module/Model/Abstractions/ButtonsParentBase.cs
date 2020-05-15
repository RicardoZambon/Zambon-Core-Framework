using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ButtonsParentBase<TButton> : SerializeNodeBase, IButtonsParent<TButton>
        where TButton : ButtonBase
    {
        #region Constants

        private const string BUTTONS_NODE = "Button";

        #endregion

        #region XML Elements

        [XmlElement(BUTTONS_NODE)]
        public ChildItemCollection<TButton> ButtonsList { get; set; }

        #endregion

        #region Constructors

        public ButtonsParentBase()
        {
            ButtonsList = new ChildItemCollection<TButton>(this);
        }

        #endregion
    }
}