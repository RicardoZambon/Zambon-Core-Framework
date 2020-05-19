using Zambon.Core.Module.Interfaces.Models.Common;
using Zambon.Core.Module.Interfaces.Models.Validations;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ILanguage : IParent, IIcon, ISettingsValidation
    {
        string Code { get; set; }

        string DisplayName { get; set; }
    }
}