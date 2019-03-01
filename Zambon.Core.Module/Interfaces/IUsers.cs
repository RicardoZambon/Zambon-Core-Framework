using System;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    public interface IUsers : IDBObject
    {

        #region Properties

        Enums.AuthenticationType AuthenticationType { get; set; }


        string FullName { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        bool LogonAllowed { get; set; }

        string Email { get; set; }

        bool SendEmail { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime? LastActivityOn { get; set; }


        bool IsAdministrator { get; }

        #endregion
        
        #region Methods

        bool UserHasAccessToType(string typeFullName, int _access);

        bool UserHasAccessToMenuID(string _menuID, string _menuType);

        #endregion

    }
}