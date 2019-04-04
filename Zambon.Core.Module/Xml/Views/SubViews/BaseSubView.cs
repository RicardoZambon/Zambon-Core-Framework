using System.Xml.Serialization;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    /// <summary>
    /// Base class used for SubViews.
    /// </summary>
    public abstract class BaseSubView : XmlNode
    {
        /// <summary>
        /// The SubView Id, used to merge same elements across ApplicationModels.
        /// </summary>
        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        /// <summary>
        /// The ViewId this SubView should show, must exist.
        /// </summary>
        [XmlAttribute("ViewId")]
        public string ViewId { get; set; }

        /// <summary>
        /// The ViewId of the parent view. Automatically set from the application.
        /// </summary>
        [XmlIgnore]
        public string ParentViewId { get; set; }

        /// <summary>
        /// The View object from the ViewId property.
        /// </summary>
        [XmlIgnore]
        public BaseView View { get; private set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            if (this is DetailModal)
                View = app.FindDetailView(ViewId);
            else if (this is LookupModal)
                View = app.FindLookupView(ViewId);
            else if (this is SubListView)
                View = app.FindListView(ViewId);

            base.OnLoadingXml(app, ctx);
        }

        internal override void OnLoadingUserModel(Application app, CoreDbContext ctx)
        {
            if (this is DetailModal)
                View = app.FindDetailView(ViewId);
            else if (this is LookupModal)
                View = app.FindLookupView(ViewId);
            else if (this is SubListView)
                View = app.FindListView(ViewId);

            base.OnLoadingUserModel(app, ctx);
        }

        #endregion
    }
}