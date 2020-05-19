using System.Xml.Serialization;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Interfaces.Models.Validations;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Languages
{
    public class Language : SerializeNodeBase, ILanguage
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

        #region Methods

        public virtual void Validate(AppSettings settings)
        {
        }

        #endregion
    }
}