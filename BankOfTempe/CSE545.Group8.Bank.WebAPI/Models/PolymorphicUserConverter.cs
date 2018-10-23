using CSE545.Group8.Bank.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSE545.Group8.Bank.WebAPI.Models
{
    public class PolymorphicUserConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Entities.User);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            Entities.User user;
            var pt = obj["type"];
            if (pt == null)
            {
                throw new ArgumentException("Missing productType", "productType");
            }

            string userType = pt.Value<string>();
            if (userType == UserType.Individual.ToString())
            {
                user = new Individual();
            }
            else if (userType == UserType.Merchant.ToString())
            {
                user = new Merchant();
            }
            else if (userType == UserType.SystemManager.ToString())
            {
                user = new SystemManager();
            }
            else if(userType == UserType.Employee.ToString())
            {
                user = new Employee();
            }
            else if(userType == UserType.Administrator.ToString())
            {
                user = new Administrator();
            }
            else
            {
                throw new NotSupportedException("Unknown user type: " + userType);
            }
            serializer.Populate(obj.CreateReader(), user);
            return user;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}