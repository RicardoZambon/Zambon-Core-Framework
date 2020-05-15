namespace Zambon.Core.Module.Interfaces.Models.Common
{
    public interface ICondition
    {
        string Condition { get; set; }

        string ConditionArguments { get; set; }
    }
}