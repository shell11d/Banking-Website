using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class Administrator : SystemManager
    {
        public Administrator() : base(UserType.Administrator)
        {
        }

        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteTransaction(int transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
