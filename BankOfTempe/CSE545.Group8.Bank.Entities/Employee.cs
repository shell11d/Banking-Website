using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class Employee : User
    {
        public int EmployeeId;

        public Employee() : base(UserType.Employee)
        {
        }

        protected Employee(UserType type) : base(type)
        {
        }


        public User ViewUserDetails(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> ViewAccountTransactions(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
