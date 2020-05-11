namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface to use with all serialization nodes from XML.
    /// </summary>
    public interface ISerializationNode
    {
        /// <summary>
        /// The parent node instance.
        /// </summary>
        object Parent { get; set; }
    }
}