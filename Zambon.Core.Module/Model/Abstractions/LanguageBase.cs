using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class LanguageBase : ILanguage
    {
        #region XML Attributes

        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string DisplayName { get; set; }

        [XmlElement]
        public object Parent { get; set; }

        [XmlElement]
        public string Icon { get; set; }

        #endregion
    }
}