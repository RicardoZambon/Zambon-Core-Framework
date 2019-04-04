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
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = "User & Password")]
            UserPassword = 0,
            /// <summary>
            /// 
            /// </summary>
            LDAP = 1
        }

        /// <summary>
        /// Types of permission.
        /// </summary>
        public enum PermissionTypes
        {
            /// <summary>
            /// 
            /// </summary>
            FullAccess = 0,
            /// <summary>
            /// 
            /// </summary>
            ReadWrite = 1, //Navigate, Read, Create, Write
            /// <summary>
            /// 
            /// </summary>
            ReadOnly = 2, //Navigate, Read
            /// <summary>
            /// 
            /// </summary>
            Navigate = 3,
            /// <summary>
            /// 
            /// </summary>
            Read = 4,
            /// <summary>
            /// 
            /// </summary>
            Create = 5,
            /// <summary>
            /// 
            /// </summary>
            Write = 6,
            /// <summary>
            /// 
            /// </summary>
            Delete = 7
        }
    }
}