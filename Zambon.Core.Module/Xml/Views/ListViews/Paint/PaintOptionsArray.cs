using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    /// <summary>
    /// Represents a node <PaintOptions></PaintOptions> from XML Application Model.
    /// </summary>
    public class PaintOptionsArray : XmlNode
    {

        /// <summary>
        /// Represent elements <PaintOption /> from XML Application Model.
        /// </summary>
        [XmlElement("PaintOption")]
        public PaintOption[] PaintOption { get; set; }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetApplicableBackColor(ExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.BackColor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetApplicableForeColor(ExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.ForeColor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetApplicableCssClass(ExpressionsService service, object obj)
        {
            return string.Join(' ', service.GetApplicableItems(PaintOption, obj).Select(x => x.CssClass));
        }

        #endregion

    }
}