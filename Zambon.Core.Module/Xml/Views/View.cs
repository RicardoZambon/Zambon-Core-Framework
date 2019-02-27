using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Xml.Views.Buttons;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views
{
    public class View : BaseView
    {

        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        [XmlIgnore]
        public Button[] Buttons { get { return _Buttons?.Button; } }

        [XmlElement("SubViews")]
        public SubViews.SubViews SubViews { get; set; }


        [XmlElement("Buttons"), Browsable(false)]
        public Buttons.Buttons _Buttons { get; set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

            if (string.IsNullOrWhiteSpace(ControllerName) && !string.IsNullOrWhiteSpace(Entity.DefaultController))
                ControllerName = Entity.DefaultController;
        }

        #endregion

        #region Methods

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

        #endregion

    }
}