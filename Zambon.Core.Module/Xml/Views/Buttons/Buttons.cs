using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.Buttons
{
    public class Buttons : XmlNode
    {

        [XmlElement("Button")]
        public Button[] Button { get; set; }

        #region Overrides

        internal override void OnLoading(Application app, CoreDbContext ctx)
        {
            if ((Button?.Length ?? 0) > 0)
                Array.Sort(Button);

            base.OnLoading(app, ctx);
        }


        #endregion

    }
}