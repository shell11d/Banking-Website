using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class SystemManager : Employee
    {

        public SystemManager() : base(UserType.SystemManager)
        {
        }

        protected SystemManager(UserType type) : base(type)
        {
        }

        public List<User> ViewAllUsers()
        {
            throw new NotImplementedException();
        }

        public List<Transaction> ViewAllTransactions(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void ChangeAccountStatus(int accountId, AccountStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
