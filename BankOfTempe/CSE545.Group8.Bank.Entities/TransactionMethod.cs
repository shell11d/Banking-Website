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
    public enum TransactionMethod
    {
        Invalid = 0,
        Creditcard = 1,
        Debitcard = 2,
        Paycheck = 3
    }
}
