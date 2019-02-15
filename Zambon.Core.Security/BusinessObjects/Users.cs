using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Validations;

namespace Zambon.Core.Security.BusinessObjects
{
    [DefaultProperty("Username")]
    public class Users : DBObject, IUsers
    {

        #region Properties

        [Display(Name = "Authentication type")]
        public Module.Helper.Enums.AuthenticationType AuthenticationType { get; set; }


        [Display(Name = "Full name"), StringLength(100)]
        [RuleRequired]
        public string FullName { get; set; }

        [StringLength(100)]
        [RuleRequired, RuleUniqueValue]
        public string Username { get; set; }

        [StringLength(100), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Logon allowed")]
        public bool LogonAllowed { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [Display(Name = "Send email?")]
        public bool SendEmail { get; set; }


        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Last activity on")]
        public DateTime LastActivityOn { get; set; }

        [Display(Name = "User is administrator")]
        public bool IsAdministrator { get { return Roles.Any(r => r.Role?.IsAdministrative ?? false); } }

        #endregion

        #region Relationships

        [Display(Name = "Roles this user is inserted")]
        public virtual ICollection<RolesUsers> Roles { get; set; }

        public virtual ICollection<UsersManagers> Managers { get; set; }

        public virtual ICollection<UsersManagers> Subordinates { get; set; }

        #endregion

        #region Overrides

        public override void ConfigureEntity(EntityTypeBuilder entity)
        {
            base.ConfigureEntity(entity);

            entity.Property("SendEmail").HasDefaultValueSql("0");
            entity.Property("LogonAllowed").HasDefaultValueSql("0");
            entity.Property("CreatedOn").HasDefaultValueSql("GETDATE()");
            entity.Property("CreatedOn").Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            entity.Property("CreatedOn").Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
        }

        #endregion

        #region Methods

        public bool UserHasAccessToType(string typeFullName, int _access)
        {
            var access = (Module.Helper.Enums.PermissionTypes)_access;
            return !string.IsNullOrWhiteSpace(typeFullName)
                && (IsAdministrator
                    || Roles?.Where(
                    r => r.Role is Roles && ((Roles)r.Role).Permissions != null && ((Roles)r.Role).Permissions.Where(
                        p => p.Entity == typeFullName
                            && ((access == Module.Helper.Enums.PermissionTypes.Create && p.CanCreate)
                                || (access == Module.Helper.Enums.PermissionTypes.Delete && p.CanDelete)
                                || (access == Module.Helper.Enums.PermissionTypes.Navigate && p.CanNavigate)
                                || (access == Module.Helper.Enums.PermissionTypes.Read && p.CanRead)
                                || (access == Module.Helper.Enums.PermissionTypes.Write && p.CanWrite))
                        ).Count() > 0
                    ).Count() > 0
                );
        }

        public bool UserHasAccessToMenuID(string _menuID, string _menuType)
        {
            if (!IsAdministrator)
            {
                if (_menuType == "ListView")
                    return (Roles.Where(r => r.Role is Roles && (((Roles)r.Role).MenuAccess?.Where(x => x.MenuId == _menuID && !x.AllowAccess)?.Count() ?? 0) == 0).Count() > 0);
                else
                    return (Roles.Where(r => r.Role is Roles && (((Roles)r.Role).MenuAccess?.Where(x => x.MenuId == _menuID && x.AllowAccess)?.Count() ?? 0) > 0).Count() > 0);
            }
            return true;
        }


        //[DbFunction(Schema = "dbo", FunctionName = "fn_IsEmployeeSubordinateOf")]
        //public static bool IsEmployeeSubordinateOf(int EmployeeID, int ManagerID)
        //{
        //    return false;// throw new InvalidOperationException($"{nameof(IsEmployeeSubordinateOf)} should be performed on database");
        //}

        //public bool IsInRole(Roles _role)
        //{
        //    return IsInRole(_role?.ID ?? -1);
        //}
        //public bool IsInRole(int? _roleID)
        //{
        //    return _roleID <= 0 || _roleID == null ? false : (Roles.Count(x => x.RoleId == _roleID) > 0);
        //}
        //public bool IsInRole(CoreContext _ctx, Roles _role)
        //{
        //    return IsInRole(_ctx, _role?.ID ?? -1);
        //}
        //public bool IsInRole(CoreContext _ctx, int _roleID)
        //{
        //    if (_roleID <= 0) return false;
        //    //LoadForeignKeys(_ctx, new[] { "Roles" });
        //    return IsInRole(_roleID);
        //}

        //private static int[] GetSubordinatesRecursive(CoreContext _ctx, int managerID)
        //{
        //    var list = new List<int>();
        //    var subordinates = _ctx.Set<UsersManagers>().Where(x => x.ManagerID == managerID).Select(x => x.SubordinateID);
        //    foreach (var subordinate in subordinates)
        //    {
        //        list.Add((int)subordinate);
        //        list.AddRange(GetSubordinatesRecursive(_ctx, (int)subordinate));
        //    }
        //    return list.ToArray();
        //}

        //public bool IsManagerOf(int _subordinateId)
        //{
        //    return SubordinateIDs != null && Array.Exists(SubordinateIDs, m => m == _subordinateId);
        //}

        #endregion

    }
}