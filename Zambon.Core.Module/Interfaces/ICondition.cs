namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interfaced used by objects that can apply conditions.
    /// </summary>
    public interface ICondition
    {

        /// <summary>
        /// The condition to search, can reference arguments by @0, @1, etc.
        /// </summary>
        string Condition { get; set; }

        /// <summary>
        /// Arguments separated by ",".
        /// </summary>
        string ConditionArguments { get; set; }

    }
}