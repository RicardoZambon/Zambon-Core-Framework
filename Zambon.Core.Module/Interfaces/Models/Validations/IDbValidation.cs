using Zambon.Core.Database;

namespace Zambon.Core.Module.Interfaces.Models.Validations
{
    public interface IDbValidation
    {
        void Validate(CoreDbContext coreDbContext);
    }
}