using System.Xml.Serialization;

namespace Zambon.Core.WebModule.Model.Entities
{
    public class Entity : Module.Model.Entities.Entity
    {
        [XmlAttribute]
        public string DefaultController { get; set; }
    }
}