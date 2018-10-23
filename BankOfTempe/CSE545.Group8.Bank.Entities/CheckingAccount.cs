using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class CheckingAccount : Account
    {
        public Card DebitCard;

        public CheckingAccount() : base()
        {
            
        }
    }
}
