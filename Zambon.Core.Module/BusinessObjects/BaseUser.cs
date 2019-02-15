//using Zambon.Core.Database;
//using Zambon.Core.Database.Entity;
//using Zambon.Core.Module.Validations;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Text;

//namespace Zambon.Core.Module.BusinessObjects
//{
//    [DefaultProperty("Username")]
//    public abstract class BaseUser : DBObject
//    {

//        #region Variables

//        protected bool _hasUpdatedPassword { get; set; }

//        #endregion

//        #region Properties

//        [Display(Name = "Full Name"), StringLength(100)]
//        [RuleRequired]
//        public string FullName { get; set; }

//        [Display(Name = "Domain Name"), StringLength(100)]
//        public string DomainName { get; set; }

//        [Display(Name = "Username Domain Prefix"), StringLength(100)]
//        public string UsernameDomainPrefix { get; set; }

//        [StringLength(100)]
//        [RuleRequired, RuleUniqueValue]
//        public string Username { get; set; }

//        [StringLength(100), DataType(DataType.Password)]
//        public string Password { get; set; }

//        [Display(Name = "Logon Allowed?")]
//        public bool LogonAllowed { get; set; }

//        [Display(Name = "Created On")]
//        public DateTime CreatedOn { get; set; }

//        #endregion

//        public virtual void FillSubordinatesArray(CoreContext _ctx)
//        {
            
//        }

//        public virtual bool UserHasAccessToType(string typeFullName, int _access)
//        {
//            return false;
//        }

//        public virtual bool HasMenuIdAccess(string _menuID, bool _permission)
//        {
//            return false;
//        }

//        public virtual bool UserHasAccessToMenuID(string _menuID, string _menuType)
//        {
//            return false;
//        }

//        public void UpdatePassword(string newPassword)
//        {
//            Password = newPassword;
//            _hasUpdatedPassword = true;
//        }

//        public virtual bool HasRolePropertyDefined<TRole>(string propertyName) where TRole : BaseRole
//        {
//            return false;
//        }

//    }
//}