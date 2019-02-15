using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zambon.Core.Module.Services.InstanceObjects
{
    public class DetailViewInstance
    {

        public object CurrentObject { get; set; }

        public string CurrentView { get; set; }

        public string Title { get; set; }


        public DetailViewInstance()
        {

        }

        public DetailViewInstance(object currentObject)
        {
            CurrentObject = currentObject;
        }

        public DetailViewInstance(string currentView)
        {
            CurrentView = currentView;
        }

    }
}