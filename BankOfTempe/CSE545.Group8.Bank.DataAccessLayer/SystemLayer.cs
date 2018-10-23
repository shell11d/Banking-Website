using BankofasuContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CSE545.Group8.Bank.Entities;

namespace CSE545.Group8.Bank.DataAccessLayer
{
    public class SystemLayer
    {
        public List<Entities.Systemlog> getSystemLogs()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var systemLogs = new List<Entities.Systemlog>();
                using (var context = new BankofasuDataContext())
                {
                    //(Entities.TransactionMethod)Enum.Parse(typeof(Entities.TransactionMethod), result.TransactionMethod)

                    var results = from logs in context.SystemLogs
                                  select logs;

                    if (results.Count() > 0)
                    {
                        foreach (var result in results)
                        {
                            var ind_transaction = new Entities.Systemlog();
                            ind_transaction = ParseSystemLogs(result);
                            systemLogs.Add(ind_transaction);
                        }
                    }
                    else {
                        return null;
                    }

                }

                scope.Complete();
                return systemLogs;
            }
        }

        public List<Entities.Systemlog> getSystemLogsByDate(DateTime startDate, DateTime endDate)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var systemLogs = new SystemLog();
                var list = new List<Entities.Systemlog>();
                using (var context = new BankofasuDataContext())
                {
                    //(Entities.TransactionMethod)Enum.Parse(typeof(Entities.TransactionMethod), result.TransactionMethod)

                    var results = from logs in context.SystemLogs
                                  where systemLogs.Time >= startDate && systemLogs.Time <= endDate
                                  select logs;

                    if (results.Count() > 0)
                    {
                        foreach (var result in results)
                        {
                            var log = ParseSystemLogs(result);
                            list.Add(log);
                        }
                    }
                    else
                    {
                        return null;
                    }

                }

                scope.Complete();
                return list;
            }
        }

        private static Systemlog ParseSystemLogs(SystemLog result)
        {
            if (result != null)
            {
                return new Entities.Systemlog
                {
                    log_id = result.LogId,
                    Description = result.Description,
                    Time = (DateTime)result.Time,
                    user_id = Guid.Parse(result.UserId),
                };
            }
            else
            {
                return null;
            }
        }
    }
}
