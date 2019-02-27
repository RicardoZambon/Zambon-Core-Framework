using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Module.Expressions;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    public class PaintOptionsArray : XmlNode
    {

        [XmlElement("PaintOption")]
        public PaintOption[] PaintOption { get; set; }

        #region Methods

        public string GetApplicableBackColor(GlobalExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.BackColor));
        }

        public string GetApplicableForeColor(GlobalExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.ForeColor));
        }

        public string GetApplicableCssClass(GlobalExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.CssClass));
        }

        #endregion

    }
}