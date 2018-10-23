using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSE545.Group8.Bank.WebAPI.Models
{
    public class SecurityQuestionsBindingModels
    {
        public class VerifySecurityInfoModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [Display(Name = "Answer1")]
            public string Answer1 { get; set; }

            [Required]
            [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [Display(Name = "Answer2")]
            public string Answer2 { get; set; }

            [Required]
            [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [Display(Name = "Answer3")]
            public string Answer3 { get; set; }

        }
    }
}