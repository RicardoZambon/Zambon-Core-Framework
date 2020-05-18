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
        #region XML Elements

        //[XmlElement]
        //public ChildItemCollection<TDetailView> DetailViews { get; set; }

        //[XmlElement]
        //public ChildItemCollection<TListView> ListViews { get; set; }

        //[XmlElement]
        //public ChildItemCollection<TLookupView> LookupViews { get; set; }

        #endregion

        #region Constructors

        public Views()
        {
            //DetailViews = new ChildItemCollection<TDetailView>(this);
            //ListViews = new ChildItemCollection<TListView>(this);
            //LookupViews = new ChildItemCollection<TLookupView>(this);
        }

        #endregion
    }
}