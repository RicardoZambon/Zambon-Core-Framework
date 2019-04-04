using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Xml.Views.Buttons;
using Zambon.Core.Module.Xml.Views.DetailViews.Scripts;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views.DetailViews
{
    /// <summary>
    /// Represents views showing detailed data.
    /// </summary>
    public class DetailView : BaseView, IViewControllerAction, IViewButtons, IViewSubViews
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
        /// The type of the view, if set to Single will only show the first element from the database. Default is empty.
        /// </summary>
        [XmlAttribute("ViewType")]
        public string ViewType { get; set; }

        /// <summary>
        /// Enctype should be used from the <form></form> HTML element. Default is empty.
        /// </summary>
        [XmlAttribute("FormEnctype")]
        public string FormEnctype { get; set; }

        /// <summary>
        /// Override the view .cshtml default name from DetailViewConfiguration node.
        /// </summary>
        [XmlAttribute("DefaultView")]
        public string DefaultView { get; set; }

        /// <summary>
        /// If Views folder is organized in three levels, should use this property. {ViewFolder}\ControllerName\ViewName.cshtml
        /// </summary>
        [XmlAttribute("ViewFolder")]
        public string ViewFolder { get; set; }


        /// <summary>
        /// List all buttons.
        /// </summary>
        [XmlIgnore]
        public Button[] Buttons { get { return _Buttons?.Button; } }

        /// <summary>
        /// List of all scripts in this detail view.
        /// </summary>
        [XmlIgnore, Browsable(false)]
        public Script[] Scripts { get { return _Scripts?.Script; } }

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

        /// <summary>
        /// List of all scripts in this detail view.
        /// </summary>
        [XmlElement("Scripts")]
        public ScriptsArray _Scripts { get; set; }


        /// <summary>
        /// The current displayed view path. {ViewFolder}\ControllerName\ViewName.cshtml
        /// </summary>
        [XmlIgnore]
        public string CurrentView { get; private set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

            if (string.IsNullOrWhiteSpace(ControllerName) && !string.IsNullOrWhiteSpace(Entity.DefaultController))
                ControllerName = Entity.DefaultController;

            if (string.IsNullOrWhiteSpace(ActionName))
                ActionName = app.ModuleConfiguration.DetailViewDefaults.DefaultAction;


            if (string.IsNullOrWhiteSpace(DefaultView))
                DefaultView = app.ModuleConfiguration.DetailViewDefaults.DefaultView;


            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
                for (var d = 0; d < SubViews.DetailViews.Length; d++)
                    SubViews.DetailViews[d].ParentViewId = ViewId;

            if ((SubViews?.LookupViews?.Length ?? 0) > 0)
                for (var l = 0; l < SubViews.LookupViews.Length; l++)
                    SubViews.LookupViews[l].ParentViewId = ViewId;

            if ((SubViews?.SubListViews?.Length ?? 0) > 0)
                for (var s = 0; s < SubViews.SubListViews.Length; s++)
                    SubViews.SubListViews[s].ParentViewId = ViewId;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the EntityType object and set the CurrentObject property.
        /// </summary>
        /// <param name="ctx">The CoreDbContext database instance.</param>
        public void ActivateInstance(CoreDbContext ctx)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(ActivateInstance) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { ctx });
        }
        /// <summary>
        /// Creates an instance of the EntityType object and set the CurrentObject property.
        /// </summary>
        /// <param name="ctx">The CoreDbContext database instance.</param>
        public void ActivateInstance<T>(CoreDbContext ctx) where T : class
        {
            T detailObject = null;
            if (typeof(T).ImplementsInterface<IEntity>())
            {
                if (ViewType?.ToLower() == "single")
                {
                    if (!string.IsNullOrEmpty(Entity.FromSql))
                        detailObject = ctx.Set<T>().FromSql(Entity.FromSql).FirstOrDefault();
                    else
                        detailObject = ctx.Set<T>().FirstOrDefault();

                    if (detailObject == null)
                        detailObject = ctx.CreateProxy<T>();
                }
                else
                    detailObject = ctx.CreateProxy<T>();
            }
            else if (typeof(T).ImplementsInterface<IEntity>())
            {
                if (!string.IsNullOrEmpty(Entity.FromSql))
                    detailObject = ctx.Set<T>().FromSql(Entity.FromSql).FirstOrDefault();
                else
                    throw new ApplicationException($"The DetailView \"{ViewId}\" has an entity \"{Entity}\" that implements the IQuery interface and is mandatory inform the attribute FromSql in Entity definition.");
            }
            else
                detailObject = (T)typeof(T).Assembly.CreateInstance(typeof(T).FullName);

            CurrentObject = detailObject;
        }

        /// <summary>
        /// Set the current view.
        /// </summary>
        /// <param name="currentView"></param>
        public void SetCurrentView(string currentView)
        {
            CurrentView = currentView;
        }

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