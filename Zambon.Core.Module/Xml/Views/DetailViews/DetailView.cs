using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.SubViews;

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

        #region Overrides

        internal override void OnLoading(Application app, CoreContext ctx)
        {
            base.OnLoading(app, ctx);

            if (string.IsNullOrWhiteSpace(DefaultView))
                DefaultView = "_Modal";

            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
                for (var d = 0; d < SubViews.DetailViews.Length; d++)
                {
                    SubViews.DetailViews[d].ParentViewId = ViewId;
                    SubViews.DetailViews[d].LoadView(app);
                }

            if ((SubViews?.LookupViews?.Length ?? 0) > 0)
                for (var l = 0; l < SubViews.LookupViews.Length; l++)
                {
                    SubViews.LookupViews[l].ParentViewId = ViewId;
                    SubViews.LookupViews[l].LoadView(app);
                }

            if ((SubViews?.SubListViews?.Length ?? 0) > 0)
                for (var s = 0; s < SubViews.SubListViews.Length; s++)
                {
                    SubViews.SubListViews[s].ParentViewId = ViewId;
                    SubViews.SubListViews[s].LoadView(app);
                }
        }

        public void ActivateInstance(ApplicationService _app, CoreContext _ctx)
        {
            typeof(DetailView).GetMethods().FirstOrDefault(x => x.Name == "ActivateInstance" && x.GetGenericArguments().Any()).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, _ctx });
        }
        public void ActivateInstance<T>(ApplicationService _app, CoreContext _ctx) where T : class
        {
            object detailObject = null;

            if (typeof(DBObject).IsAssignableFrom(typeof(T)))
            {
                //Database object
                if (ViewType?.ToLower() == "single")
                {
                    detailObject = _ctx.Set<T>().FirstOrDefault();
                    if (detailObject == null)
                        detailObject = _ctx.CreateProxy<T>();
                }
                else 
                    detailObject = _ctx.CreateProxy<T>();
            }
            else
                detailObject = typeof(T).Assembly.CreateInstance(typeof(T).FullName);

            _app.SetDetailViewCurrentObject(ViewId, detailObject);
        }

        #endregion

    }
}