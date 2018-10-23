using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace CSE545.Group8.Bank.Entities
{
    public class UserInfo
    {
        [Required]
        public Guid UserId;

        [Required]
        public string SecurityQuestion1 {get; set;}

        [Required]
        public string SecurityAnswer1 { get; set; }

        [Required]
        public string SecurityQuestion2 { get; set; }

        [Required]
        public string SecurityAnswer2 { get; set; }

        public string SecurityQuestion3 {get; set;}

        public string SecurityAnswer3 { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
