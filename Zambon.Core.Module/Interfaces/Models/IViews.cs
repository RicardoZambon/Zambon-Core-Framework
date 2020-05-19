using Zambon.Core.Module.Interfaces.Models.Validations;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IViews : IParent, IModelValidation
    {
    }

    public interface IViews<TDetailView, TListView, TLookupView> : IViews
        where TDetailView : IDetailView
        where TListView : IListView
        where TLookupView : ILookupView
    {
        ChildItemCollection<TDetailView> DetailViews { get; set; }

        ChildItemCollection<TListView> ListViews { get; set; }

        ChildItemCollection<TLookupView> LookupViews { get; set; }
    }
}