namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IProperty : IParent
    {
        string Name { get; set; }

        string DisplayName { get; set; }

        string Prompt { get; set; }

        string Description { get; set; }
    }
}