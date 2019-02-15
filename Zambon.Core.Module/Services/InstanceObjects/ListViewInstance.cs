using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zambon.Core.Module.Services.InstanceObjects
{
    public class ListViewInstance
    {

        public SearchOptions SearchOptions { get; set; }

        public IQueryable<BaseDBObject> ItemsCollection { get; set; }

        public BaseDBObject CurrentObject { get; set; }


        public ListViewInstance(IQueryable<BaseDBObject> itemsCollection)
        {
            ItemsCollection = itemsCollection;
        }

        public ListViewInstance(BaseDBObject currentObject)
        {
            CurrentObject = currentObject;
        }

        public ListViewInstance(SearchOptions searchOptions)
        {
            SearchOptions = searchOptions;
        }


    }
}