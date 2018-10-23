using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankofasuContext;
using CSE545.Group8.Bank.Entities;

namespace CSE545.Group8.Bank.DataAccessLayer
{
    public class UserLayer
    {
        public static List<Individual> GetAllIndividualUsers()
        {
            using (var context = new BankofasuDataContext())
            {
                var results = from customer in context.CustomerInfos
                              where customer.Type == UserType.Individual.ToString()
                              select customer;

                if (results.Any())
                {
                    var individualUserList = new List<Individual>();

                    foreach (var result in results)
                    {
                        var newIndividualUser = ParseIndividual(result);
                        individualUserList.Add(newIndividualUser);
                    }
                    return individualUserList;
                }
            }
            return null;
        }

        public static List<Merchant> GetAllMerchantsWithAccounts()
        {
            var merchantList = new List<Merchant>();
            using (var context = new BankofasuDataContext())
            {
                var results = from customer in context.CustomerInfos
                              where customer.Type == Entities.UserType.Merchant.ToString()
                              select new
                              {
                                  customer,
                                  accounts = from account in context.Accounts
                                             where account.CustomerId == customer.Id
                                             select account
                              };

                foreach (var result in results)
                {
                    var newMerchant = ParseMerchantWithAccounts(result.customer, result.accounts.ToList());
                    merchantList.Add(newMerchant);
                }

                return merchantList;
            }
        }

       

        private static Merchant ParseMerchantWithAccounts(CustomerInfo merchantResult, List<BankofasuContext.Account> accounts)
        {
            var newMerchant = new Merchant();
            newMerchant.FirstName = merchantResult.FirstName;
            newMerchant.LastName = merchantResult.LastName;
            newMerchant.FullName = merchantResult.FullName;
            newMerchant.UserId = Guid.Parse(merchantResult.Id);
            newMerchant.JoiningDate = merchantResult.JoiningDt ?? DateTime.MinValue;
            newMerchant.Status = (UserStatus)Enum.Parse(typeof(UserStatus), merchantResult.Status);
            newMerchant.Gender = merchantResult.Gender;
            newMerchant.DateOfBirth = merchantResult.DOB ?? DateTime.MinValue;
            newMerchant.Address = merchantResult.Address;
            newMerchant.PhoneNumber = merchantResult.Phone.ToString();
            newMerchant.EmailAddress = merchantResult.Email;
            newMerchant.Ssn = merchantResult.SSN != null ? merchantResult.SSN.Value : 0;
            newMerchant.Accounts = new List<Entities.Account>();

            foreach (var account in accounts)
            {
                if (account != null)
                {
                    if (account.AccountType == AccountType.Checking.ToString())
                        newMerchant.Accounts.Add(AccountsLayer.ParseCheckingAccount(account));
                    if (account.AccountType == AccountType.Savings.ToString())
                        newMerchant.Accounts.Add(AccountsLayer.ParseSavingsAccount(account));
                }
            }

            return newMerchant;
        }

        public static List<Entities.Employee> GetAllEmployees()
        {
            var employeeList = new List<Entities.Employee>();
            using (var context = new BankofasuDataContext())
            {
                var databaseEmployee = from employee in context.EmployeeInfos
                                       where employee.Type == Entities.UserType.Employee.ToString()
                                       select employee;
                foreach (var result in databaseEmployee)
                {
                    var newEmployee = ParseEmployee(result);
                    employeeList.Add(newEmployee);
                }
            }
            return employeeList;
        }

        public static Entities.Individual GetIndividualUserByID(Guid userID)
        {
            var newIndividualUser = new Entities.Individual();
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from customer in context.CustomerInfos
                                       where customer.Id == userID.ToString() && customer.Type == Entities.UserType.Individual.ToString()
                                       select customer;
                var result = databaseCustomer.FirstOrDefault();
                newIndividualUser = ParseIndividual(result);
            }
            return newIndividualUser;
        }

        public static Entities.Merchant GetMerchantByID(Guid userID)
        {
            var newMerchant = new Entities.Merchant();
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from customer in context.CustomerInfos
                                       where customer.Id == userID.ToString() && customer.Type == Entities.UserType.Merchant.ToString()
                                       select customer;
                var result = databaseCustomer.FirstOrDefault();
                newMerchant = ParseMerchant(result);
            }
            return newMerchant;
        }

        public static Entities.Employee GetEmployeeByID(Guid userID)
        {
            var newEmployee = new Entities.Employee();
            using (var context = new BankofasuDataContext())
            {
                var databaseEmployee = from employee in context.EmployeeInfos
                                       where employee.Id == userID.ToString() && employee.Type == Entities.UserType.Employee.ToString()
                                       select employee;
                var result = databaseEmployee.FirstOrDefault();
                newEmployee = ParseEmployee(result);
            }
            return newEmployee;
        }

        public static Entities.SystemManager GetSystemManagerByID(Guid userID)
        {
            var newSystemManager = new Entities.SystemManager();
            using (var context = new BankofasuDataContext())
            {
                var databaseEmployee = from employee in context.EmployeeInfos
                                       where employee.Id == userID.ToString() && employee.Type == Entities.UserType.SystemManager.ToString()
                                       select employee;
                var result = databaseEmployee.FirstOrDefault();
                newSystemManager = (Entities.SystemManager)ParseEmployee(result);
            }
            return newSystemManager;
        }

        public static Entities.Administrator GetAdministratorByID(Guid userID)
        {
            var newAdministrator = new Entities.Administrator();
            using (var context = new BankofasuDataContext())
            {
                var databaseEmployee = from employee in context.EmployeeInfos
                                       where employee.Id == userID.ToString() && employee.Type == Entities.UserType.Administrator.ToString()
                                       select employee;
                var result = databaseEmployee.FirstOrDefault();
                newAdministrator = (Entities.Administrator)ParseEmployee(result);
            }
            return newAdministrator;
        }

        public static Entities.User GetUserByPhoneNumber(string phoneNumber)
        {
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from customer in context.CustomerInfos
                                       where customer.Phone == phoneNumber
                                       select customer;
                var customerSearchResult = databaseCustomer.FirstOrDefault();
                if (customerSearchResult != null)
                    return GetParsedUserFromCustomerInfo(customerSearchResult);

                var databaseEmployee = from employee in context.EmployeeInfos
                                       where employee.Phone == phoneNumber
                                       select employee;
                var employeeSearchResult = databaseEmployee.FirstOrDefault();

                if (employeeSearchResult != null)
                    return GetParsedUserFromEmployeeInfo(employeeSearchResult);
            }
            return null;
        }

        public static User GetUserByEmailId(string email)
        {
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from customer in context.CustomerInfos
                                       where customer.Email == email
                                       select customer;
                var customerSearchResult = databaseCustomer.FirstOrDefault();
                if (customerSearchResult != null)
                    return GetParsedUserFromCustomerInfo(customerSearchResult);

                var databaseEmployee = from employee in context.EmployeeInfos
                                       where employee.Email == email
                                       select employee;
                var employeeSearchResult = databaseEmployee.FirstOrDefault();

                if (employeeSearchResult != null)
                    return GetParsedUserFromEmployeeInfo(employeeSearchResult);
            }
            return null;
        }

        private static Entities.User GetParsedUserFromEmployeeInfo(EmployeeInfo employeeResult)
        {
            Entities.User user;
            var userType = (Entities.UserType)Enum.Parse(typeof(Entities.UserType), employeeResult.Type);
            user = GetUserByType(userType);

            user.FirstName = employeeResult.FirstName;
            user.LastName = employeeResult.LastName;
            user.FullName = employeeResult.FullName;
            user.UserId = Guid.Parse(employeeResult.Id);
            user.JoiningDate = employeeResult.JoiningDt ?? DateTime.MinValue;
            user.Status = (Entities.UserStatus)Enum.Parse(typeof(Entities.UserStatus), employeeResult.Status);
            user.Gender = employeeResult.Gender;
            user.DateOfBirth = employeeResult.DOB ?? DateTime.MinValue;
            user.Address = employeeResult.Address;
            user.PhoneNumber = employeeResult.Phone.ToString();
            user.EmailAddress = employeeResult.Email;
            user.Ssn = employeeResult.SSN != null ? employeeResult.SSN.Value : 0;

            return user;
        }

        private static User GetUserByType(UserType userType)
        {

            switch (userType)
            {
                case (Entities.UserType.SystemManager):
                    {
                        return new Entities.SystemManager();
                    }
                case (Entities.UserType.Administrator):
                    {
                        return new Entities.Administrator();
                    }
                case (Entities.UserType.Merchant):
                    {
                        return new Entities.Merchant();
                    }
                case (Entities.UserType.Individual):
                    {
                        return new Entities.Individual();
                    }
                default:
                    {
                        return new Entities.Employee();
                    }
            }
        }

        private static Entities.User GetParsedUserFromCustomerInfo(BankofasuContext.CustomerInfo customerResult)
        {
            var userType = (Entities.UserType)Enum.Parse(typeof(Entities.UserType), customerResult.Type);
            var user = GetUserByType(userType);

            user.FirstName = customerResult.FirstName;
            user.LastName = customerResult.LastName;
            user.FullName = customerResult.FullName;
            user.UserId = Guid.Parse(customerResult.Id);
            user.JoiningDate = customerResult.JoiningDt ?? DateTime.MinValue;
            user.Status = (Entities.UserStatus)Enum.Parse(typeof(Entities.UserStatus), customerResult.Status);
            user.Gender = customerResult.Gender;
            user.DateOfBirth = customerResult.DOB ?? DateTime.MinValue;
            user.Address = customerResult.Address;
            user.PhoneNumber = customerResult.Phone.ToString();
            user.EmailAddress = customerResult.Email;
            user.Ssn = customerResult.SSN != null ? customerResult.SSN.Value : 0;

            return user;
        }

        public static Entities.Individual GetIndividualUserByPhoneNumber(string phoneNumber)
        {
            var newIndividualUser = new Entities.Individual();
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from customer in context.CustomerInfos
                                       where customer.Phone == phoneNumber && customer.Type == Entities.UserType.Individual.ToString()
                                       select customer;
                var result = databaseCustomer.FirstOrDefault();
                newIndividualUser = ParseIndividual(result);
            }
            return newIndividualUser;
        }

        public static Entities.Individual GetIndividualUserByEmailID(string email)
        {
            var newIndividualUser = new Entities.Individual();
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from customer in context.CustomerInfos
                                       where customer.Email == email && customer.Type == Entities.UserType.Individual.ToString()
                                       select customer;
                var result = databaseCustomer.FirstOrDefault();
                newIndividualUser = ParseIndividual(result);
            }
            return newIndividualUser;
        }

        public static Guid GetCustomerIDByEmail(string email)
        {
            Guid customerID;
            using (var context = new BankofasuDataContext())
            {
                var resultID = from customer in context.CustomerInfos
                               where customer.Email == email
                               select customer.Id;
                customerID = Guid.Parse(resultID.ToString());
            }
            return customerID;
        }

        public static Guid GetEmployeeIDByEmail(string email)
        {
            Guid employeeID;
            using (var context = new BankofasuDataContext())
            {
                var resultID = from employee in context.EmployeeInfos
                               where employee.Email == email
                               select employee.Id;
                employeeID = Guid.Parse(resultID.ToString());
            }
            return employeeID;
        }

        public static CustomerInfo GetCustomerToPut(User user)
        {
            if (user != null)
            {
                var customer = new CustomerInfo
                {
                    Id = user.UserId.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    JoiningDt = DateTime.Now,
                    Status = user.Status.ToString(),
                    Gender = user.Gender,
                    DOB = user.DateOfBirth,
                    Address = user.Address,
                    Email = user.EmailAddress,
                    Phone = user.PhoneNumber,
                    SSN = user.Ssn,
                    Type = user.Type.ToString()
                };

                return customer;
            }
            else
                return null;
        }

        public static BankofasuContext.EmployeeInfo GetEmployeeToPut(User user)
        {
            if (user != null)
            {
                var customer = new EmployeeInfo
                {
                    Id = user.UserId.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    JoiningDt = DateTime.Now,
                    Status = user.Status.ToString(),
                    Gender = user.Gender,
                    DOB = user.DateOfBirth,
                    Address = user.Address,
                    Email = user.EmailAddress,
                    Phone = user.PhoneNumber,
                    SSN = user.Ssn,
                    Type = user.Type.ToString()
                };

                return customer;
            }
            else
                return null;
        }
        public static bool PutCustomer(User user)
        {
            using (var context = new BankofasuDataContext())
            {
                var customer = GetCustomerToPut(user);
                if (customer != null)
                    context.CustomerInfos.InsertOnSubmit(customer);
                else
                    return false;
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

        public static bool PutEmployee(User user)
        {
            using (var context = new BankofasuDataContext())
            {
                var employee = GetEmployeeToPut(user);
                if (employee != null)
                    context.EmployeeInfos.InsertOnSubmit(employee);
                else
                    return false;
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
        public static bool PostCustomer(Guid userID, User user)
        {
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from customer in context.CustomerInfos
                                       where customer.Id == userID.ToString()
                                       select customer;
                var result = databaseCustomer.FirstOrDefault();
                if (result != null)
                    result = GetUserToPost(result, user);
                else
                    return false;
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

        public static bool PostEmployee(Guid userID, User user)
        {
            using (var context = new BankofasuDataContext())
            {
                var databaseCustomer = from employee in context.EmployeeInfos
                                       where employee.Id == userID.ToString()
                                       select employee;
                var result = databaseCustomer.FirstOrDefault();


                if (result != null)
                    result = GetEmployeeToPost(result, user);
                else
                    return false;
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

        private static CustomerInfo GetUserToPost(CustomerInfo result, User user)
        {
            result.FirstName = !string.IsNullOrWhiteSpace(user.FirstName) ? user.FirstName : result.FirstName;
            result.LastName = !string.IsNullOrWhiteSpace(user.LastName) ? user.LastName : result.LastName;
            result.FullName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : result.FullName;
            result.Status = user.Status.ToString();
            result.Gender = !string.IsNullOrWhiteSpace(user.Gender) ? user.Gender : result.Gender;
            result.Address = !string.IsNullOrWhiteSpace(user.Address) ? user.Address : result.Address;
            result.Phone = !string.IsNullOrWhiteSpace(user.PhoneNumber) ? user.PhoneNumber : result.Phone;
            result.SSN = user.Ssn != 0 ? user.Ssn : result.SSN;
            return result;
        }

        private static EmployeeInfo GetEmployeeToPost(EmployeeInfo result, User user)
        {
            result.FirstName = !string.IsNullOrWhiteSpace(user.FirstName) ? user.FirstName : result.FirstName;
            result.LastName = !string.IsNullOrWhiteSpace(user.LastName) ? user.LastName : result.LastName;
            result.FullName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : result.FullName;
            result.Status = user.Status.ToString();
            result.Gender = !string.IsNullOrWhiteSpace(user.Gender) ? user.Gender : result.Gender;
            result.Address = !string.IsNullOrWhiteSpace(user.Address) ? user.Address : result.Address;
            result.Phone = !string.IsNullOrWhiteSpace(user.PhoneNumber) ? user.PhoneNumber : result.Phone;
            result.SSN = user.Ssn != 0 ? user.Ssn : result.SSN;
            return result;
        }

        private static Entities.Individual ParseIndividual(BankofasuContext.CustomerInfo individualResult)
        {
            Entities.Individual newIndividual = new Entities.Individual();

            newIndividual.FirstName = individualResult.FirstName;
            newIndividual.LastName = individualResult.LastName;
            newIndividual.FullName = individualResult.FullName;
            newIndividual.UserId = Guid.Parse(individualResult.Id);
            newIndividual.JoiningDate = individualResult.JoiningDt ?? DateTime.MinValue;
            newIndividual.Status = (Entities.UserStatus)Enum.Parse(typeof(Entities.UserStatus), individualResult.Status);
            newIndividual.Gender = individualResult.Gender;
            newIndividual.DateOfBirth = individualResult.DOB ?? DateTime.MinValue;
            newIndividual.Address = individualResult.Address;
            newIndividual.PhoneNumber = individualResult.Phone.ToString();
            newIndividual.EmailAddress = individualResult.Email;
            newIndividual.Ssn = individualResult.SSN != null ? individualResult.SSN.Value : 0;

            return newIndividual;
        }

        private static Entities.Merchant ParseMerchant(BankofasuContext.CustomerInfo merchantResult)
        {
            Entities.Merchant newMerchant = new Entities.Merchant();

            newMerchant.FirstName = merchantResult.FirstName;
            newMerchant.LastName = merchantResult.LastName;
            newMerchant.FullName = merchantResult.FullName;
            newMerchant.UserId = Guid.Parse(merchantResult.Id);
            newMerchant.JoiningDate = merchantResult.JoiningDt ?? DateTime.MinValue;
            newMerchant.Status = (Entities.UserStatus)Enum.Parse(typeof(Entities.UserStatus), merchantResult.Status);
            newMerchant.Gender = merchantResult.Gender;
            newMerchant.DateOfBirth = merchantResult.DOB ?? DateTime.MinValue;
            newMerchant.Address = merchantResult.Address;
            newMerchant.PhoneNumber = merchantResult.Phone.ToString();
            newMerchant.EmailAddress = merchantResult.Email;
            newMerchant.Ssn = merchantResult.SSN != null ? merchantResult.SSN.Value : 0;

            return newMerchant;
        }

        private static Entities.Merchant ParseMerchantWithAccounts(BankofasuContext.CustomerInfo merchantResult)
        {
            Entities.Merchant newMerchant = new Entities.Merchant();

            newMerchant.FirstName = merchantResult.FirstName;
            newMerchant.LastName = merchantResult.LastName;
            newMerchant.FullName = merchantResult.FullName;
            newMerchant.UserId = Guid.Parse(merchantResult.Id);
            newMerchant.JoiningDate = merchantResult.JoiningDt ?? DateTime.MinValue;
            newMerchant.Status = (Entities.UserStatus)Enum.Parse(typeof(Entities.UserStatus), merchantResult.Status);
            newMerchant.Gender = merchantResult.Gender;
            newMerchant.DateOfBirth = merchantResult.DOB ?? DateTime.MinValue;
            newMerchant.Address = merchantResult.Address;
            newMerchant.PhoneNumber = merchantResult.Phone.ToString();
            newMerchant.EmailAddress = merchantResult.Email;
            newMerchant.Ssn = merchantResult.SSN != null ? merchantResult.SSN.Value : 0;

            return newMerchant;
        }

        private static Entities.Employee ParseEmployee(BankofasuContext.EmployeeInfo employeeResult)
        {
            Entities.Employee employee;
            var userType = (Entities.UserType)Enum.Parse(typeof(Entities.UserType), employeeResult.Type);
            switch (userType)
            {
                case (Entities.UserType.SystemManager):
                    {
                        employee = new Entities.SystemManager();
                        break;
                    }
                case (Entities.UserType.Administrator):
                    {
                        employee = new Entities.Administrator();
                        break;
                    }
                default:
                    {
                        employee = new Entities.Employee();
                        break;
                    }
            }

            employee.FirstName = employeeResult.FirstName;
            employee.LastName = employeeResult.LastName;
            employee.FullName = employeeResult.FullName;
            employee.UserId = Guid.Parse(employeeResult.Id);
            employee.JoiningDate = employeeResult.JoiningDt ?? DateTime.MinValue;
            employee.Status = (Entities.UserStatus)Enum.Parse(typeof(Entities.UserStatus), employeeResult.Status);
            employee.Gender = employeeResult.Gender;
            employee.DateOfBirth = employeeResult.DOB ?? DateTime.MinValue;
            employee.Address = employeeResult.Address;
            employee.PhoneNumber = employeeResult.Phone.ToString();
            employee.EmailAddress = employeeResult.Email;
            employee.Ssn = employeeResult.SSN != null ? employeeResult.SSN.Value : 0;

            return employee;
        }
    }
}
