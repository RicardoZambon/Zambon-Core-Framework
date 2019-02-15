using Zambon.Core.Module.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zambon.Core.Security.Models
{
    public class Login
    {
        [RuleRequired]
        public string Username { get; set; }

        [RuleRequired]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; } = true;

        public string ReturnUrl { get; set; }

    }
}