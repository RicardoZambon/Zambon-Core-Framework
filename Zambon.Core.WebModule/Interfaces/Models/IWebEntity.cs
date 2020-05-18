using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.WebModule.Interfaces.Models
{
    public interface IWebEntity : IEntity
    {
        string DefaultController { get; set; }
    }
}