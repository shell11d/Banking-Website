using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CSE545.Group8.Bank.Entities
{
    public abstract class User
    {
        //public string UserName;
        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(128, ErrorMessage = "Maximum Length is 128 Chars")]
        public string FirstName;

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(128, ErrorMessage = "Maximum Length is 128 Chars")]
        public string LastName;

        public string FullName;

        
        public DateTime JoiningDate;

        //[Required]
       // [StringLength(32, ErrorMessage = "Maximum Length is 32 Chars")]
        public UserStatus Status;

        public Guid UserId;

        public string Gender;

        public DateTime DateOfBirth;

        [StringLength(150, ErrorMessage = "Maximum Length is 150 Chars")]
        public string Address;
        

        [Required]
        [StringLength(16, ErrorMessage = "Maximum Length is 16 Chars")]
        public string PhoneNumber;

        [Required]
        [EmailAddress]
        public string EmailAddress;

        public int Ssn;

        //public UserInfo UserInfo;

        //[Required]
       // [StringLength(32, ErrorMessage = "Maximum Length is 128 Chars")]
        public UserType Type { get { return _type; } }

        private readonly UserType _type;

        protected User(UserType type)
        {
            _type = type;
        }

        public void Login()
        {
            throw new NotImplementedException();
        }
    }
}
