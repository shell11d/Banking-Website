using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankofasuContext;
using CSE545.Group8.Bank.Entities;

namespace CSE545.Group8.Bank.DataAccessLayer
{
    public static class DataAccessLayer
    {
        public static IEnumerable<User> GetAllCustomerDetails()
        {
            var users = new List<User>();
            using (var context = new BankofasuDataContext())
            {
                var results = from a in context.CustomerInfos
                              select a;

                foreach (var result in results)
                {
                    var user = new Individual();
                    user.UserId = Guid.Parse(result.Id);
                    user.FirstName = result.FirstName;
                    user.LastName = result.LastName;
                    users.Add(user);
                }
            }
            return users;
        }
    }
}
