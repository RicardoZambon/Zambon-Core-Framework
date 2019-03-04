using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    /// <summary>
    /// Represents a node <LookupModal /> from XML Application Model.
    /// </summary>
    public class LookupModal : BaseModal
    {

        /// <summary>
        /// The LookUpView view object.
        /// </summary>
        public LookupView LookUpView { get { return View as LookupView; } }

    }
}