using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    public class PaintOptionsArray : XmlNode
    {

        [XmlElement("PaintOption")]
        public PaintOption[] PaintOption { get; set; }

        #region Methods

        public string GetApplicableBackColor(ExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.BackColor));
        }

        public string GetApplicableForeColor(ExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.ForeColor));
        }

        public string GetApplicableCssClass(ExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.CssClass));
        }

        #endregion

    }
}