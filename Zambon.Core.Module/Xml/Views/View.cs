using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Xml.Views.Buttons;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views
{
    /// <summary>
    /// Represents base properties for nodes of <DetailView></DetailView> or <ListView></ListView> from XML Application Model.
    /// </summary>
    public abstract class View : BaseView
    {

        /// <summary>
        /// The ControllerName attribute from XML. Define the default controller name to be used within this view, by default will use the same as set in EntityType.
        /// </summary>
        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        /// <summary>
        /// The ActionName attribute from XML. Define the action name to be used for this view.
        /// </summary>
        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        /// <summary>
        /// List all buttons.
        /// </summary>
        [XmlIgnore]
        public Button[] Buttons { get { return _Buttons?.Button; } }

        /// <summary>
        /// The SubViews element from XML.
        /// </summary>
        [XmlElement("SubViews")]
        public SubViews.SubViews SubViews { get; set; }

        /// <summary>
        /// The Buttons element from XML.
        /// </summary>
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

        /// <summary>
        /// Retrieves a SubView using the SubView Id.
        /// </summary>
        /// <param name="Id">The Id of the SubView.</param>
        /// <returns>If found, return the SubView instance; Otherwise, return null.</returns>
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