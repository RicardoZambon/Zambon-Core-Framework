using System.Xml.Serialization;
using Zambon.Core.Module.Model.Nodes.Entities.Properties;
using Zambon.Core.WebModule.Model.Abstractions;
using Zambon.Core.WebModule.Model.Nodes.Entities;

namespace Zambon.Core.WebModule.Model.Nodes
{
    [XmlRoot(APPLICATION_NODE)]
    public class WebApplication : WebApplicationBase<WebEntityTypes, WebEntity, Properties, Property>
    {
    }
}