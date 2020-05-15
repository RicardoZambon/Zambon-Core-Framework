using System.Xml.Serialization;
using Zambon.Core.WebModule.ModelAbstractions;

namespace Zambon.Core.WebModule.Model.Entities
{
    public class WebEntity : WebEntityBase
    {
        #region XML Attributes

        [XmlAttribute]
        public string DefaultController { get; set; }

        #endregion
    }
}