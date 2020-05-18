using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebGridTemplateBase : GridTemplateBase, IWebGridTemplate
    {
        #region XML Attributes

        [XmlAttribute]
        public string RowCssClass { get; set; }

        [XmlAttribute]
        public string CellCssClass { get; set; }

        #endregion
    }
}