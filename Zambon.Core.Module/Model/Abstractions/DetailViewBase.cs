using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class DetailViewBase<TButtonsParent, TButton> : ViewBase, IDetailView<TButtonsParent, TButton>
        where TButtonsParent : ButtonsParentBase<TButton>
            where TButton : ButtonBase
    {
        #region XML Elements

        private TButtonsParent _buttons;
        [XmlElement(nameof(Buttons)), Browsable(false)]
        public TButtonsParent _Buttons
        {
            get => _buttons;
            set => SetParent(value, ref _buttons);
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public ChildItemCollection<TButton> Buttons => _Buttons?.ButtonsList ?? new ChildItemCollection<TButton>(null);

        #endregion
    }
}