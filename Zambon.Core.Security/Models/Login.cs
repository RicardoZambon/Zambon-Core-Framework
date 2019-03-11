using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Module.Validations;

namespace Zambon.Core.Security.Models
{
    public class Login : ICustomValidated
    {
        [RuleRequired]
        public string Username { get; set; }

        [RuleRequired]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; } = true;

        public string ReturnUrl { get; set; }
    

        public List<KeyValuePair<string, string>> ValidateData(CoreDbContext ctx)
        {
            return new List<KeyValuePair<string, string>>();
        }
    }
}