using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CSE545.Group8.Bank.Entities
{
    public abstract class Account
    {

        [Required]
        public AccountType Type
        { get; set; }

        public AccountStatus Status;

        //public List<Transaction> Transactions;

        public Guid CustomerId;

        public int AccountId;

        public float AvailableBalance;

        public DateTime CreateDate;

        public DateTime CloseDate;

        protected Account()
        {
        }
    }
}
