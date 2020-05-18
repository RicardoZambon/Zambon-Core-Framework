using Zambon.Core.Module.Model.Abstractions;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebGridTemplatesParentBase<TGridTemplate> : GridTemplatesParentBase<TGridTemplate>
        where TGridTemplate : WebGridTemplateBase
    {
        #region Constructors

        public WebGridTemplatesParentBase() : base()
        {
        }

        #endregion
    }
}