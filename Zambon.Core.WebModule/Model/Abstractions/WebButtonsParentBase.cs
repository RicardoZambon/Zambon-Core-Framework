using Zambon.Core.Module.Model.Abstractions;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebButtonsParentBase<TButton> : ButtonsParentBase<TButton>
        where TButton : WebButtonBase
    {
        #region Constructors

        public WebButtonsParentBase() : base()
        {
        }

        #endregion
    }
}