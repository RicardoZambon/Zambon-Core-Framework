namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IStaticText : IParent
    {
        string Id { get; set; }

        string Value { get; set; }
    }
}