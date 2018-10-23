using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class Merchant : Customer
    {
        public Merchant() : base(UserType.Merchant)
        {
        }
    }
}
