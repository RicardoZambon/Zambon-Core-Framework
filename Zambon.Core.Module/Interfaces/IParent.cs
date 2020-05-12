namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Represents a node that have a parent node.
    /// </summary>
    public interface IParent
    {
        /// <summary>
        /// The parent node instance.
        /// </summary>
        object Parent { get; set; }
    }
}