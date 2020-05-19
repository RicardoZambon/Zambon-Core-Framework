using Zambon.Core.Module.Configurations;

namespace Zambon.Core.Module.Interfaces.Models.Validations
{
    public interface ISettingsValidation
    {
        void Validate(AppSettings settings);
    }
}