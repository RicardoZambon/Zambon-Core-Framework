namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IStaticText : IParent
    {
        string Key { get; set; }

        string Value { get; set; }
    }
}