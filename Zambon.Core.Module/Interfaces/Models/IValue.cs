namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IValue : IParent
    {
        string Key { get; set; }

        string DisplayName { get; set; }
    }
}