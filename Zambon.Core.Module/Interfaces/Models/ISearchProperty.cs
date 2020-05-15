namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ISearchProperty : IParent
    {
        string PropertyName { get; set; }

        string DisplayName { get; set; }
    }
}