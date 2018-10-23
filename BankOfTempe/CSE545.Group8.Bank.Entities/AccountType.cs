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
    public enum AccountType
    {
        Invalid = 0,
        Savings = 1,
        Checking = 2,
        CreditCard = 3

    }
}
