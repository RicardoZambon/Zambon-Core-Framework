using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ILanguage : IParent, IIcon
    {
        string Code { get; set; }

        string DisplayName { get; set; }
    }
}