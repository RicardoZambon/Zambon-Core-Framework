using System;
using Zambon.Core.Database;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface used to map basic user properties.
    /// </summary>
    public interface IUsers : IDBObject
    {

        #region Properties

        /// <summary>
        /// Controls how this user should be authenticated when accessing the system.
        /// </summary>
        Enums.AuthenticationType AuthenticationType { get; set; }


        /// <summary>
        /// The user complete name.
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// The username the user should user to logon.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// The user password ecripted if the authentication method is 0 (UserPassword), otherwise can be null null.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// When false the user is not able to logon.
        /// </summary>
        bool LogonAllowed { get; set; }

        /// <summary>
        /// The user current email.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Indication if the system should send emails or not.
        /// </summary>
        bool SendEmail { get; set; }

        /// <summary>
        /// Date/time when this user was created.
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Date/time indicating the last acitivity in the system.
        /// </summary>
        DateTime? LastActivityOn { get; set; }


        /// <summary>
        /// If this user is inserted in any IsAdministrative roles, will return true.
        /// </summary>
        bool IsAdministrator { get; }

        #endregion
        
        #region Methods

        /// <summary>
        /// Checks if the user has the specific access to the type.
        /// </summary>
        /// <param name="typeFullName">The type to search.</param>
        /// <param name="_access">The access to check.</param>
        /// <returns>If the user has access will return true.</returns>
        bool UserHasAccessToType(string typeFullName, int _access);

        /// <summary>
        /// Checks if the user has access to the menu item.
        /// </summary>
        /// <param name="_menuID">The menu ID.</param>
        /// <param name="_menuType">The menu type.</param>
        /// <returns>If the user has access will return true.</returns>
        bool UserHasAccessToMenuID(string _menuID, string _menuType);

        /// <summary>
        /// Loads current user related data.
        /// </summary>
        /// <param name="ctx">The EF DBContext instance.</param>
        void RefreshCurrentUserData(CoreDbContext ctx);

        #endregion

    }
}