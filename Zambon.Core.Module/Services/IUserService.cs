using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    /// <summary>
    /// Interface to implements in methods responsible to the handle the current logged user.
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// The current identity logged.
        /// </summary>
        string CurrentIdentityName { get; }

        /// <summary>
        /// The object of the curent logged user.
        /// </summary>
        IUsers CurrentUser { get; }

        /// <summary>
        /// Checks if the current user has changed.
        /// </summary>
        void CheckUserChanged();
        
    }
}