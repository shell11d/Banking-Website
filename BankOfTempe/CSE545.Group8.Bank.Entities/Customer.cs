using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class Customer : User
    {
        public List<Account> Accounts;
        public Customer(UserType type) : base(type)
        {
        }

        public virtual IEnumerable<Account> GetAccounts()
        {
            return new List<Account> { new SavingsAccount(), new CheckingAccount(), new CreditCardAccount() };
        }

        public virtual int ViewFunds(int accountId)
        {
            throw new NotImplementedException();
        }

        public virtual int ActivateAccount(int accountId)
        {
            throw new NotImplementedException();
        }
        public virtual int TransferFunds(float amount, int receiverAccountId)
        {
            throw new NotImplementedException();
        }

        public virtual int ViewTransactions(DateTime startDate, DateTime endDate, int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
