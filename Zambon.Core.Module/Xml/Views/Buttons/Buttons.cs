using System;
using System.Xml.Serialization;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Views.Buttons
{
    public class Buttons : XmlNode
    {

        [XmlElement("Button")]
        public Button[] Button { get; set; }

        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            if ((Button?.Length ?? 0) > 0)
                Array.Sort(Button);

            base.OnLoadingXml(app, ctx);
        }

        #endregion

    }
}