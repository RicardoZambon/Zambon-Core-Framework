using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    public class PaintOptions : XmlNode
    {

        [XmlElement("PaintOption")]
        public PaintOption[] PaintOption { get; set; }

        #region Methods

        public string GetApplicableBackColor(object _obj, GlobalExpressionsService _service)
        {
            return string.Join(' ', _service.GetApplicableExpressionsItems(PaintOption, _obj)?.Select(x => ((PaintOption)x).BackColor) ?? new string[0]) ?? string.Empty;
        }
        public string GetApplicableForeColor(object _obj, GlobalExpressionsService _service)
        {
            return string.Join(' ', _service.GetApplicableExpressionsItems(PaintOption, _obj)?.Select(x => ((PaintOption)x).ForeColor) ?? new string[0]) ?? string.Empty;
        }
        public string GetApplicableCustomClass(object _obj, GlobalExpressionsService _service)
        {
            return string.Join(' ', _service.GetApplicableExpressionsItems(PaintOption, _obj)?.Select(x => ((PaintOption)x).CustomClass) ?? new string[0]) ?? string.Empty;
        }


        #endregion

    }
}
