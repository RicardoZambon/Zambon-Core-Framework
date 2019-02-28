namespace Zambon.Core.Module.Interfaces
{
    public interface IUserService
    {

        string CurrentIdentityName { get; }

        IUsers CurrentUser { get; }

        void CheckUserChanged();
        
    }
}