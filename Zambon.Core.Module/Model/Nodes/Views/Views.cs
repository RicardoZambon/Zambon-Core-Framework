using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public class Views<TDetailView, TListView, TLookupView> : SerializeNodeBase, IViews<TDetailView, TListView, TLookupView>
        where TDetailView : class, IDetailView
        where TListView : class, IListView
        where TLookupView : class, ILookupView
    {
        #region Constants

        private const string DETAILVIEWS_NODE = "DetailView";
        private const string LISTVIEWS_NODE = "ListView";
        private const string LOOKUPVIEWS_NODE = "LookupView";

        #endregion

        #region XML Elements

        [XmlElement(DETAILVIEWS_NODE)]
        public ChildItemCollection<TDetailView> DetailViews { get; set; }

        [XmlElement(LISTVIEWS_NODE)]
        public ChildItemCollection<TListView> ListViews { get; set; }

        [XmlElement(LOOKUPVIEWS_NODE)]
        public ChildItemCollection<TLookupView> LookupViews { get; set; }

        #endregion

        #region Constructors

        public Views()
        {
            DetailViews = new ChildItemCollection<TDetailView>(this);
            ListViews = new ChildItemCollection<TListView>(this);
            LookupViews = new ChildItemCollection<TLookupView>(this);
        }

        #endregion

        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}