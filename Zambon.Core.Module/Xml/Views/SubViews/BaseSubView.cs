using System.Xml.Serialization;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public abstract class BaseSubView : XmlNode
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("ViewId")]
        public string ViewId { get; set; }

        [XmlIgnore]
        public string ParentViewId { get; set; }

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