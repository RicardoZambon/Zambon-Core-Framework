using System.Xml.Serialization;
using Zambon.Core.Module.Model.Nodes.Views.GridTemplates;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Nodes.Views.GridTemplates
{
    public class WebGridTemplate : GridTemplate, IWebGridTemplate
    {
        #region XML Attributes

        [XmlAttribute]
        public string RowCssClass { get; set; }

        [XmlAttribute]
        public string CellCssClass { get; set; }

        #endregion
    }
}