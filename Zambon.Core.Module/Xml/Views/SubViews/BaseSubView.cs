using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public abstract class BaseSubView : XmlNode
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("ViewId")]
        public string ViewId { get; set; }

        [XmlIgnore]
        public string ParentViewId { get; set; }

        [XmlIgnore]
        public BaseView View { get; private set; }


        #region Methods

        public virtual void LoadView(Application _app)
        {
            if (this is DetailModal)
                View = _app.FindDetailView(ViewId);
            else if (this is LookupModal)
                View = _app.FindLookupView(ViewId);
            else if (this is SubListView)
                View = _app.FindListView(ViewId);
        }

        #endregion

    }
}
