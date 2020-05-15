using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Entities;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;
using Zambon.Core.Module.Model.Nodes.Navigation;
using Zambon.Core.Module.Model.Nodes.StaticTexts;

namespace Zambon.Core.Module.Model.Nodes
{
    [XmlRoot]
    public class Application : ApplicationBase<EntityTypesParent, Entity, PropertiesParent, Property, StaticTextsParent, StaticText, NavigationParent, Menu>
    {
    }
}