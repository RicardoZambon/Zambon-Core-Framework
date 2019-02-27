using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Xml.Views.DetailViews
{
    public class DetailView : View
    {
        [XmlAttribute("ViewType")]
        public string ViewType { get; set; }

        [XmlAttribute("FormEnctype")]
        public string FormEnctype { get; set; }

        [XmlAttribute("DefaultView")]
        public string DefaultView { get; set; }

        [XmlAttribute("ViewFolder")]
        public string ViewFolder { get; set; }

        [XmlElement("Scripts")]
        public Scripts.Scripts Scripts { get; set; }


        [XmlIgnore]
        public object CurrentObject { get; private set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

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

        public void ActivateInstance(CoreDbContext ctx)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(ActivateInstance) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { ctx });
        }
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

        #endregion

    }
}