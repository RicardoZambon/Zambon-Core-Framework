using System.Xml.Serialization;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRootNode
    {
        XmlAttributeOverrides GetXmlAttributeOverrides();
    }
}