using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankofasuContext;
using CSE545.Group8.Bank.Entities;

namespace CSE545.Group8.Bank.DataAccessLayer
{
    public static class UserInfoLayer
    {
        public static Entities.UserInfo GetUserInfoByUserId(Guid userID)
        {
            var newUserInfo = new Entities.UserInfo();
            using(var context = new BankofasuDataContext())
            {
                var databaseUserInfo = from user in context.UserSecurityQuestions
                                       join customer in context.CustomerInfos
                                       on user.Id equals customer.Id
                                       where user.Id == userID.ToString()
                                       select new {user, customer.DOB};

                var result = databaseUserInfo.FirstOrDefault();
                if(result!=null)
                {
                    var userResult = result.user;
                        var dobResult = result.DOB ?? DateTime.MinValue;
                    return ParseUserInfo(userResult, dobResult);
                }
                               
            }
            return null;
        }


        public static Entities.UserInfo GetUserInfoByEmail(string email)
        {
            var newUserInfo = new Entities.UserInfo();
            using (var context = new BankofasuDataContext())
            {
                var databaseUserInfo = from user in context.UserSecurityQuestions
                                       join customer in context.CustomerInfos
                                       on user.Id equals customer.Id
                                       where customer.Email == email
                                       select new { user, customer.DOB };

                var result = databaseUserInfo.FirstOrDefault();
                if (result != null)
                {
                    var userResult = result.user;
                    var dobResult = result.DOB ?? DateTime.MinValue;
                    return ParseUserInfo(userResult, dobResult);
                }

            }
            return null;
        }

        public static Entities.UserInfo GetUserQuestionsByEmail(string userEmail)
        {
            using (var context = new BankofasuDataContext())
            {
                var databaseUserInfo = from user in context.UserSecurityQuestions
                                       join customer in context.CustomerInfos
                                       on user.Id equals customer.Id
                                       where customer.Email == userEmail
                                       select new { user, customer.DOB };

                var result = databaseUserInfo.FirstOrDefault();
                if (result != null)
                {
                    var userResult = result.user;
                    var dobResult = result.DOB ?? DateTime.MinValue;
                    return ParseUserQuestionsInfo(userResult, dobResult);
                }
            }
            return null;
        }

        private static UserInfo ParseUserQuestionsInfo(UserSecurityQuestion securityResult, DateTime dobResult)
        {
            if (securityResult != null)
            {
                return new Entities.UserInfo
                {
                    UserId = Guid.Parse(securityResult.Id),
                    SecurityQuestion1 = securityResult.Q1.ToString(),
                    SecurityQuestion2 = securityResult.Q2.ToString(),
                    SecurityQuestion3 = securityResult.Q3.ToString(),
                    DateOfBirth = dobResult
                };
            }
            else
                return null;
        }

        public static bool PutUserInfo(Entities.UserInfo newInfo)
        {
            using (var context = new BankofasuDataContext())
            {
                UserSecurityQuestion info = new UserSecurityQuestion
                {
                    Id = newInfo.UserId.ToString(),
                    Q1 = newInfo.SecurityQuestion1,
                    A1 = newInfo.SecurityAnswer1,
                    Q2 = newInfo.SecurityQuestion2,
                    A2 = newInfo.SecurityAnswer2,
                    Q3 = newInfo.SecurityQuestion3,
                    A3 = newInfo.SecurityAnswer3
                };
                context.UserSecurityQuestions.InsertOnSubmit(info);
                try
                {
                    context.SubmitChanges();
                }

                catch (Exception e)
                {
                    return false;
                }

                return true;
            }
        }

        private static Entities.UserInfo ParseUserInfo(UserSecurityQuestion securityResult, DateTime dobResult)
        {
            if (securityResult != null)
            {
                return new Entities.UserInfo
                {
                    UserId = Guid.Parse(securityResult.Id),
                    SecurityQuestion1 = securityResult.Q1.ToString(),
                    SecurityAnswer1 = securityResult.A1,
                    SecurityQuestion2 = securityResult.Q2.ToString(),
                    SecurityAnswer2 = securityResult.A2,
                    SecurityQuestion3 = securityResult.Q3.ToString(),
                    SecurityAnswer3 = securityResult.A3,
                    DateOfBirth = dobResult
                };
            }
            else
                return null;
        }
    }
}
