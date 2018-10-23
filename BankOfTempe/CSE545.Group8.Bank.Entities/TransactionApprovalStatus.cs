using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionApprovalStatus
    {
        Pending = 0,
        ReceiverApproved = 1,
        BankerApproved = 2,
        Approved = 3,
        Declined = 4,
    }
}
