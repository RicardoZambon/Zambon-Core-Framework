using System.Linq;
using System.Linq.Dynamic.Core;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    /// <summary>
    /// Represents a node <LookupView></LookupView> from XML Application Model.
    /// </summary>
    public class LookupView : BaseListView
    {
        /// <summary>
        /// The post back options the LookUpView should use when submitting back to the parent view.
        /// </summary>
        [XmlIgnore]
        public PostBackOptions PostBackOptions { get; protected set; }


        #region Methods

        /// <summary>
        /// Set the LookUpView contents items.
        /// </summary>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <param name="searchOptions">If applying search, otherwise null.</param>
        public void PopulateView(ApplicationService app, CoreDbContext ctx, SearchOptions searchOptions = null)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(PopulateView) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, ctx, searchOptions });
        }
        /// <summary>
        /// Set the LookUpView contents items.
        /// </summary>
        /// <typeparam name="T">The type of the LookUpView.</typeparam>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <param name="searchOptions">If applying search, otherwise null.</param>
        public void PopulateView<T>(ApplicationService app, CoreDbContext ctx, SearchOptions searchOptions = null) where T : class
        {
            //Todo: Validate if still needed. GC.Collect();
            var list = GetPopulatedView<T>(app, ctx, searchOptions);

            CurrentObject = null;
            ItemsCollection = list;
        }

        /// <summary>
        /// Set the value of the PostBackOptions property.
        /// </summary>
        /// <param name="postBackOptions">The PostBackOptions object.</param>
        public void SetPostBackOptions(PostBackOptions postBackOptions)
        {
            PostBackOptions = postBackOptions;
        }

        #endregion
    }
}