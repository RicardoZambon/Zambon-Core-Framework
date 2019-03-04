namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interfaced used by objects that can apply crterias.
    /// </summary>
    public interface ICriteria
    {

        /// <summary>
        /// The criteria apply, can reference arguments by @0, @1, etc.
        /// </summary>
        string Criteria { get; set; }

        /// <summary>
        /// Arguments separated by ",".
        /// </summary>
        string CriteriaArguments { get; set; }

    }
}