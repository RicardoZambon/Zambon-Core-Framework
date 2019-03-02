using System;
using System.Xml.Serialization;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Views.Buttons
{
    /// <summary>
    /// Represents a list of buttons used in DetailViews or ListViews.
    /// </summary>
    public class Buttons : XmlNode
    {

        /// <summary>
        /// List of all buttons.
        /// </summary>
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