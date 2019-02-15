using Zambon.Core.Database;
using Zambon.Core.Module.Xml.Views.SubViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views
{
    public class View : BaseView
    {

        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        [XmlElement("Buttons")]
        public Buttons.Buttons Buttons { get; set; }

        [XmlElement("SubViews")]
        public SubViews.SubViews SubViews { get; set; }


        public BaseSubView GetSubView(string Id)
        {
            BaseSubView view = null;
            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
                view = Array.Find(SubViews.DetailViews, m => m.Id == Id);

            if (view == null && (SubViews?.LookupViews?.Length ?? 0) > 0)
                view = Array.Find(SubViews.LookupViews, m => m.Id == Id);

            if (view == null && (SubViews?.SubListViews?.Length ?? 0) > 0)
            {
                view = Array.Find(SubViews.SubListViews, m => m.Id == Id);

                if (view == null)
                    for (var s = 0; s < SubViews.SubListViews.Length; s++)
                    {
                        view = SubViews.SubListViews[s].ListView.GetSubView(Id);
                        if (view != null)
                            return view;
                    }
            }

            return view;
        }

    }
}