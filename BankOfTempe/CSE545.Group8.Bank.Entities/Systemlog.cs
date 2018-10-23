using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class Systemlog
    {
        public int log_id;
        public string Description;
        public DateTime Time;
        public Guid user_id;
    }
}
