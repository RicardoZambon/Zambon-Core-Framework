using Zambon.Core.Module.Xml.Views.DetailViews;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    /// <summary>
    /// Modal used to show DetailViews.
    /// </summary>
    public class DetailModal : BaseModal
    {
        /// <summary>
        /// The DetailView object from the ViewId property.
        /// </summary>
        public DetailView DetailView { get { return View as DetailView; } }
    }
}