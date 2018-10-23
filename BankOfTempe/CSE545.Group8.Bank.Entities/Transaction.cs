using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CSE545.Group8.Bank.Entities
{
    public  class Transaction
    {
        [Required]
        public TransactionType Type;

        public int TransactionId;


        public DateTime TransactionDate;

        public string Description;

        [Required]
        public float Amount;


        public TransactionStatus Status;

        [Required]
        public int FromAccountId;

        [Required]
        public int ToAccountId;

        [Required]
        public int AccountId;

        [Required]
        public TransactionMethod TransactionMethod;

        public TransactionApprovalStatus Approval_Status;

        /* true for critical, false for non critical */
        public bool critical;

    }
}
