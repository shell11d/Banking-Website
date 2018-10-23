using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class CreditCardAccount : Account
    {
        public Card CreditCard;

        public float CreditLimit;

        public float MinimumDueAmount;

        public DateTime DueDate;

        public DateTime LastPaymentDate;

        public float LastDueAmount;

        public float LastPaidAmount;

        public CreditCardAccount() : base()
        {
        }
    }
}
