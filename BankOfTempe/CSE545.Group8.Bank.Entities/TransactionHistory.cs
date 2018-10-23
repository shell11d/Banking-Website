using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class TransactionHistory
    {
        public int AccountId;

        public List<Transaction> Transactions;

        public DateTime StartDate;

        public DateTime EndDate;
    }
}
