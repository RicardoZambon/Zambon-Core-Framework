using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using Zambon.Core.Module.Xml.Views.SubViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zambon.Core.Module.Services.InstanceObjects
{
    public class LookUpViewInstance
    {

        public SearchOptions SearchOptions { get; set; }

        public IQueryable<BaseDBObject> ItemsCollection { get; set; }

        public BaseDBObject CurrentObject { get; set; }

        public PostBackOptions PostBackOptions { get; set; }


        public LookUpViewInstance(IQueryable<BaseDBObject> itemsCollection)
        {
            ItemsCollection = itemsCollection;
        }

        public LookUpViewInstance(BaseDBObject currentObject)
        {
            CurrentObject = currentObject;
        }

        public LookUpViewInstance(SearchOptions searchOptions)
        {
            SearchOptions = searchOptions;
        }

        public LookUpViewInstance(PostBackOptions postBackOptions)
        {
            PostBackOptions = postBackOptions;
        }

    }
}