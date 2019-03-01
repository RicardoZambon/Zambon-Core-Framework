using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    public interface IUserService
    {

        string CurrentIdentityName { get; }

        IUsers CurrentUser { get; }

        void CheckUserChanged();
        
    }
}