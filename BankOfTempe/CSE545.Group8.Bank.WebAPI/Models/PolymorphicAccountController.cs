using CSE545.Group8.Bank.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSE545.Group8.Bank.WebAPI.Models
{
    public class PolymorphicAccountController : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Entities.User);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            Account account;
            var pt = obj["type"];
            if (pt == null)
            {
                throw new ArgumentException("Missing productType", "productType");
            }

            string accountType = pt.Value<string>();
            if (accountType == AccountType.Checking.ToString())
            {
                account = new CheckingAccount();
            }
            else if (accountType == AccountType.Savings.ToString())
            {
                account = new SavingsAccount();
            }
            else if (accountType == AccountType.CreditCard.ToString())
            {
                account = new CreditCardAccount();
            }
            
            else
            {
                throw new NotSupportedException("Unknown user type: " + accountType);
            }
            serializer.Populate(obj.CreateReader(), account);
            return account;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}