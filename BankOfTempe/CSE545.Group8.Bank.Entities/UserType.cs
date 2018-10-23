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
    public enum UserType
    {
        Invalid = 0,
        Individual = 1,
        Merchant = 2,
        Administrator = 3,
        Employee = 4,
        SystemManager = 5
    }
}
