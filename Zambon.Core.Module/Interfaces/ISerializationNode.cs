namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface to use with all serialization nodes from XML.
    /// </summary>
    public interface ISerializationNode
    {
        void Merge(object readObj);
    }
}