using System.ComponentModel.DataAnnotations;

namespace Zambon.Core.Module
{
    /// <summary>
    /// Enums used in Module.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Types the user can authenticate.
        /// </summary>
        public enum AuthenticationType
        {
            [Display(Name = "User & Password")]
            UserPassword = 0,
            LDAP = 1
        }

        /// <summary>
        /// Types of permission.
        /// </summary>
        public enum PermissionTypes
        {
            FullAccess = 0,
            ReadWrite = 1, //Navigate, Read, Create, Write
            ReadOnly = 2, //Navigate, Read
            Navigate = 3,
            Read = 4,
            Create = 5,
            Write = 6,
            Delete = 7
        }

    }
}