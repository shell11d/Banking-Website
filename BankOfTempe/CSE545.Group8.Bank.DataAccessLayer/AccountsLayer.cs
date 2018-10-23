using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankofasuContext;
using System.Transactions;
using CSE545.Group8.Bank.Entities;

namespace CSE545.Group8.Bank.DataAccessLayer
{
    public class AccountsLayer
    {
        public static List<CheckingAccount> GetAllCheckingAccountsByUserID(Guid userID)
        {
            var newAccountList = new List<Entities.CheckingAccount>();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           join card in context.Debitcards
                                           on account.CustomerId equals card.CustomerId
                                           where account.CustomerId == userID.ToString() && account.AccountType == Entities.AccountType.Checking.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select new { account, card };

                    if (databaseAccounts.Count() != 0)
                    {
                        foreach (var dbAccount in databaseAccounts)
                        {
                            var accountResult = dbAccount.account;
                            var cardResult = dbAccount.card;
                            var newAccount = ParseCheckingAccount(accountResult, cardResult);
                            newAccountList.Add(newAccount);
                        }
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccountList;
            }
        }

        public static Entities.CheckingAccount GetCheckingAccountByUserID(Guid userID)
        {
            var newAccount = new Entities.CheckingAccount();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           join card in context.Debitcards
                                           on account.CustomerId equals card.CustomerId
                                           where account.CustomerId == userID.ToString() && account.AccountType == Entities.AccountType.Checking.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select new { account, card };

                    var dbAccount = databaseAccounts.FirstOrDefault();
                    if (dbAccount != null)
                    {
                        var accountResult = dbAccount.account;
                        var cardResult = dbAccount.card;
                        newAccount = ParseCheckingAccount(accountResult, cardResult);
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccount;
            }
        }

        public static List<Entities.SavingsAccount> GetAllSavingsAccountsByUserID(Guid userID)
        {
            var newAccountList = new List<Entities.SavingsAccount>();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           where account.CustomerId == userID.ToString() && account.AccountType == Entities.AccountType.Savings.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select account;
                    if (databaseAccounts.Count() != 0)
                    {
                        foreach (var dbAccount in databaseAccounts)
                        {
                            var newAccount = ParseSavingsAccount(dbAccount);
                            newAccountList.Add(newAccount);
                        }
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccountList;
            }
        }

        public static Entities.SavingsAccount GetSavingsAccountByUserID(Guid userID)
        {
            var newAccount = new Entities.SavingsAccount();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           where account.CustomerId == userID.ToString() && account.AccountType == Entities.AccountType.Savings.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select account;
                    var dbAccount = databaseAccounts.FirstOrDefault();
                    if (dbAccount != null)
                    {
                        newAccount = ParseSavingsAccount(dbAccount);
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccount;
            }
        }

        public static List<Entities.CreditCardAccount> GetAllCreditCardAccountsByUserID(Guid userID)
        {
            var newAccountList = new List<Entities.CreditCardAccount>();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           join card in context.CreditcardDetails
                                           on account.CustomerId equals card.CustomerId
                                           where account.CustomerId == userID.ToString() && account.AccountType == Entities.AccountType.CreditCard.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select new { account, card };
                    if (databaseAccounts.Count() != 0)
                    {
                        foreach (var dbAccount in databaseAccounts)
                        {
                            var accountResult = dbAccount.account;
                            var cardResult = dbAccount.card;
                            var newAccount = ParseCreditCardAccount(accountResult, cardResult);
                            newAccountList.Add(newAccount);
                        }
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccountList;
            }
        }

        public static Entities.CreditCardAccount GetCreditCardAccountByUserID(Guid userID)
        {
            var newAccount = new Entities.CreditCardAccount();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           join card in context.CreditcardDetails
                                           on account.CustomerId equals card.CustomerId
                                           where account.CustomerId == userID.ToString() && account.AccountType == Entities.AccountType.CreditCard.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select new { account, card };
                    var dbAccount = databaseAccounts.FirstOrDefault();
                    if (dbAccount != null)
                    {
                        var accountResult = dbAccount.account;
                        var cardResult = dbAccount.card;
                        newAccount = ParseCreditCardAccount(accountResult, cardResult);
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccount;

            }
        }



        public static Entities.CheckingAccount GetCheckingAccountByAccountID(int accountID)
        {
            var newAccount = new Entities.CheckingAccount();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           join card in context.Debitcards
                                           on account.AccountId equals card.AccountId
                                           where account.AccountId == accountID && account.AccountType == Entities.AccountType.Checking.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select new { account, card };

                    if (databaseAccounts.Count() != 0)
                    {
                        var accountResult = databaseAccounts.FirstOrDefault().account;
                        var cardResult = databaseAccounts.FirstOrDefault().card;
                        newAccount = ParseCheckingAccount(accountResult, cardResult);
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccount;
            }
        }

        public static Entities.SavingsAccount GetSavingsAccountByAccountID(int accountID)
        {
            var newAccount = new Entities.SavingsAccount();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           where account.AccountId == accountID && account.AccountType == Entities.AccountType.Savings.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select account;

                    if (databaseAccounts.Count() != 0)
                    {
                        var accountResult = databaseAccounts.FirstOrDefault();
                        newAccount = ParseSavingsAccount(accountResult);
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccount;
            }
        }

        public static Entities.CreditCardAccount GetCreditCardAccountByAccountID(int accountID)
        {
            var newAccount = new Entities.CreditCardAccount();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var databaseAccounts = from account in context.Accounts
                                           join card in context.CreditcardDetails
                                           on account.AccountId equals card.AccountId
                                           where account.AccountId == accountID && account.AccountType == Entities.AccountType.CreditCard.ToString() && account.Status != Entities.AccountStatus.Closed.ToString()
                                           select new { account, card };
                    if (databaseAccounts.Count() != 0)
                    {
                        var accountResult = databaseAccounts.FirstOrDefault().account;
                        var cardResult = databaseAccounts.FirstOrDefault().card;
                        newAccount = ParseCreditCardAccount(accountResult, cardResult);
                    }
                    else
                        return null;
                }
                scope.Complete();
                return newAccount;
            }
        }

        public static int PutCheckingAccount(CheckingAccount newAccount)
        {
            var p = -1;
            using (var scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var customer = from cust in context.CustomerInfos
                                   where cust.Id == newAccount.CustomerId.ToString()
                                   select cust;

                    var customer_check = from account in context.Accounts
                                         where account.CustomerId == newAccount.CustomerId.ToString() && account.AccountType == Entities.AccountType.Checking.ToString()
                                         select account;

                    
                    if (customer.Count() != 0 && newAccount.Type == Entities.AccountType.Checking && customer_check.Count() == 0)
                    {
                        var account = GetDBAccountToPut(newAccount);
                        var random = new Random();
                        if (account != null)
                        {


                            context.Accounts.InsertOnSubmit(account);
                            context.SubmitChanges();

                            Debitcard card = new Debitcard
                            {
                                CustomerId = newAccount.CustomerId.ToString(),
                                Name = customer.FirstOrDefault().FullName,
                                DateOfExpiry = DateTime.Now.AddYears(3),
                                DateOfJoining = DateTime.Now,
                                CVV = random.Next(101, 999),
                                Status = Entities.CardStatus.Enabled.ToString(),
                                AccountId = account.AccountId
                            };

                            context.Debitcards.InsertOnSubmit(card);
                            context.SubmitChanges();
                            p = account.AccountId;
                            scope.Complete();

                        }

                    }

                }

            }

            return p;
        }

        private static BankofasuContext.Account GetDBAccountToPut(Entities.Account newAccount)
        {

            return new BankofasuContext.Account()
            {
                AvailableBalance = (decimal)newAccount.AvailableBalance,
                CreatedOn = DateTime.Now,
                // ClosingDt = newAccount.CloseDate,
                // Status = newAccount.Status.ToString(),
                CustomerId = newAccount.CustomerId.ToString(),
                AccountType = newAccount.Type.ToString()
            };
        }

        public static int PutSavingsAccount(Entities.SavingsAccount newAccount)
        {
            var p = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var customer = from cust in context.CustomerInfos
                                   where cust.Id == newAccount.CustomerId.ToString()
                                   select cust;
                    var customer_check = from account in context.Accounts
                                         where account.CustomerId == newAccount.CustomerId.ToString() && account.AccountType == Entities.AccountType.Savings.ToString()
                                         select account;

                    if (customer.Count() != 0 && newAccount.Type == Entities.AccountType.Savings && customer_check.Count() == 0)
                    {
                        var account = GetDBAccountToPut(newAccount);
                        if (account != null)
                        {
                            context.Accounts.InsertOnSubmit(account);
                            context.SubmitChanges();
                        }
                        scope.Complete();
                        p = account.AccountId;
                    }

                }
            }
            return p;
        }
        public static int PutCreditCardAccount(Entities.CreditCardAccount newAccount)
        {
            var p = -1;
            using (var scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var customer = from cust in context.CustomerInfos
                                   where cust.Id == newAccount.CustomerId.ToString()
                                   select cust;
                    var customer_check = from account in context.Accounts
                                         where account.CustomerId == newAccount.CustomerId.ToString() && account.AccountType == Entities.AccountType.CreditCard.ToString()
                                         select account;


                    if (customer.Count() != 0 && newAccount.Type == Entities.AccountType.CreditCard && customer_check.Count() == 0)
                    {
                        var account = GetDBAccountToPut(newAccount);
                        var random = new Random();
                        if (account != null)
                        {


                            context.Accounts.InsertOnSubmit(account);
                            context.SubmitChanges();
                            CreditcardDetail card = new CreditcardDetail
                            {
                                Name = customer.FirstOrDefault().FirstName,
                                AccountId = account.AccountId,
                                CustomerId = newAccount.CustomerId.ToString(),
                                CreditLimit = 1500,
                                StartDate = DateTime.Now,
                                CVV = random.Next(101, 999),
                                Status = Entities.CardStatus.Enabled.ToString(),
                                ExpiryDt = DateTime.Now.AddYears(3)
                            };
                            context.CreditcardDetails.InsertOnSubmit(card);
                            context.SubmitChanges();
                            p = account.AccountId;
                            scope.Complete();
                        }

                    }

                }

            }
            return p;
        }

        public static int PostCheckingAccount(int accountID, Entities.CheckingAccount newAccount)
        {
            var p = -1;
            using (var context = new BankofasuDataContext())
            {

                var databaseAccount = from account in context.Accounts
                                      where account.AccountId == accountID && account.AccountType == Entities.AccountType.Checking.ToString()
                                      select account;

                var accountResult = databaseAccount.FirstOrDefault();

                if (UpdateAccountForPost(accountResult, newAccount, accountID))
                {

                    var databaseCard = from card in context.Debitcards
                                       where card.AccountId == accountID
                                       select card;
                    var cardResult = databaseCard.FirstOrDefault();

                    if (cardResult != null && (newAccount.DebitCard.Status == Entities.CardStatus.Enabled || newAccount.DebitCard.Status == Entities.CardStatus.Disabled))
                    {
                        //cardResult.Name = newAccount.DebitCard.NameOnCard;
                        //cardResult.DateOfExpiry = newAccount.DebitCard.DateOfExpiry;
                        //cardResult.DateOfJoining = newAccount.DebitCard.DateOfIssue;
                        cardResult.Status = newAccount.DebitCard.Status.ToString();
                        context.SubmitChanges();
                        p = newAccount.AccountId;

                    }
                }
            }
            return p;
        }

        public static int PostSavingsAccount(int accountID, Entities.SavingsAccount newAccount)
        {
            var p = -1;
            using (var context = new BankofasuDataContext())
            {
                var databaseAccount = from account in context.Accounts
                                      where account.AccountId == accountID && account.AccountType == Entities.AccountType.Savings.ToString()
                                      select account;

                var accountResult = databaseAccount.FirstOrDefault();

                if (UpdateAccountForPost(accountResult, newAccount,accountID))
                {
                    context.SubmitChanges();
                    p = accountResult.AccountId;
                }

            }

            return p;

        }

        public static int PostCreditCardAccount(int accountID, Entities.CreditCardAccount newAccount)
        {
            var p = -1;
            using (var context = new BankofasuDataContext())
            {
                var databaseAccount = from account in context.Accounts
                                      where account.AccountId == accountID && account.AccountType == Entities.AccountType.CreditCard.ToString()
                                      select account;

                var accountResult = databaseAccount.FirstOrDefault();

                if (UpdateAccountForPost(accountResult, newAccount,accountID))
                {
                    var databaseCard = from card in context.CreditcardDetails
                                       where card.AccountId == accountID
                                       select card;

                    var cardResult = databaseCard.FirstOrDefault();
                    if (cardResult != null && (newAccount.CreditCard.Status == Entities.CardStatus.Enabled || newAccount.CreditCard.Status == Entities.CardStatus.Disabled))
                    {
                        //cardResult.Name = newAccount.CreditCard.NameOnCard;
                        //cardResult.CreditLimit = (int)newAccount.CreditLimit;
                        //cardResult.StartDate = newAccount.CreditCard.DateOfIssue;
                        //cardResult.ExpiryDt = newAccount.CreditCard.DateOfExpiry;
                        cardResult.Status = newAccount.CreditCard.Status.ToString();
                        context.SubmitChanges();
                        p = accountResult.AccountId;
                    }
                }

            }

            return p;

        }

        private static bool UpdateAccountForPost(BankofasuContext.Account accountResult, Entities.Account newAccount, int accountId)
        {
            var p = false;

            if (accountResult != null && newAccount.AvailableBalance >= 0 && newAccount.CreateDate != null && newAccount.CloseDate != null && (newAccount.Status == Entities.AccountStatus.Active || newAccount.Status == Entities.AccountStatus.Blocked || newAccount.Status == Entities.AccountStatus.Closed || newAccount.Status == Entities.AccountStatus.Created))
            {
                accountResult.AccountId = accountId;
                accountResult.AvailableBalance = (int)newAccount.AvailableBalance;
                accountResult.CreatedOn = newAccount.CreateDate;
                accountResult.ClosingDt = newAccount.CloseDate;
                accountResult.Status = newAccount.Status.ToString();
                accountResult.AccountType = newAccount.Type.ToString();
                p = true;
            }

            return p;
        }


        public static Entities.CheckingAccount ParseCheckingAccount(BankofasuContext.Account accountResult, BankofasuContext.Debitcard debitCardResult)
        {
            Entities.CheckingAccount newCheckingAccount = new Entities.CheckingAccount();

            newCheckingAccount.AccountId = (int)accountResult.AccountId;
            newCheckingAccount.CustomerId = Guid.Parse(accountResult.CustomerId);
            newCheckingAccount.Status = (Entities.AccountStatus)Enum.Parse(typeof(Entities.AccountStatus), accountResult.Status);
            newCheckingAccount.AvailableBalance = (float)accountResult.AvailableBalance;
            newCheckingAccount.CreateDate = accountResult.CreatedOn;
            newCheckingAccount.CloseDate = accountResult.ClosingDt ?? DateTime.MaxValue;
            newCheckingAccount.Type = (Entities.AccountType)Enum.Parse(typeof(Entities.AccountType), accountResult.AccountType);
            newCheckingAccount.DebitCard = new Entities.Card();
            newCheckingAccount.DebitCard.CardNumber = debitCardResult.DebitCardNumber.ToString();
            newCheckingAccount.DebitCard.Cvv = debitCardResult.CVV;
            newCheckingAccount.DebitCard.DateOfExpiry = debitCardResult.DateOfExpiry;
            newCheckingAccount.DebitCard.DateOfIssue = debitCardResult.DateOfJoining;
            newCheckingAccount.DebitCard.NameOnCard = debitCardResult.Name;
            newCheckingAccount.DebitCard.Status = (Entities.CardStatus)Enum.Parse(typeof(Entities.CardStatus), debitCardResult.Status);

            return newCheckingAccount;
        }

        public static Entities.SavingsAccount ParseSavingsAccount(BankofasuContext.Account accountResult)
        {
            Entities.SavingsAccount newSavingsAccount = new Entities.SavingsAccount();
            newSavingsAccount.AccountId = (int)accountResult.AccountId;
            newSavingsAccount.CustomerId = Guid.Parse(accountResult.CustomerId);
            newSavingsAccount.Status = (Entities.AccountStatus)Enum.Parse(typeof(Entities.AccountStatus), accountResult.Status);
            newSavingsAccount.Type = (Entities.AccountType)Enum.Parse(typeof(Entities.AccountType), accountResult.AccountType);
            newSavingsAccount.AvailableBalance = (float)accountResult.AvailableBalance;
            newSavingsAccount.CreateDate = accountResult.CreatedOn;
            newSavingsAccount.CloseDate = accountResult.ClosingDt ?? DateTime.MaxValue;

            return newSavingsAccount;
        }

        public static Entities.CheckingAccount ParseCheckingAccount(BankofasuContext.Account accountResult)
        {
            Entities.CheckingAccount newSavingsAccount = new Entities.CheckingAccount();
            newSavingsAccount.AccountId = (int)accountResult.AccountId;
            newSavingsAccount.CustomerId = Guid.Parse(accountResult.CustomerId);
            newSavingsAccount.Status = (Entities.AccountStatus)Enum.Parse(typeof(Entities.AccountStatus), accountResult.Status);
            newSavingsAccount.Type = (Entities.AccountType)Enum.Parse(typeof(Entities.AccountType), accountResult.AccountType);
            newSavingsAccount.AvailableBalance = (float)accountResult.AvailableBalance;
            newSavingsAccount.CreateDate = accountResult.CreatedOn;
            newSavingsAccount.CloseDate = accountResult.ClosingDt ?? DateTime.MaxValue;

            return newSavingsAccount;
        }

        private static Entities.CreditCardAccount ParseCreditCardAccount(BankofasuContext.Account accountResult, BankofasuContext.CreditcardDetail creditCardResult)
        {
            Entities.CreditCardAccount newCreditCardAccount = new Entities.CreditCardAccount();
            newCreditCardAccount.AccountId = (int)accountResult.AccountId;
            newCreditCardAccount.CustomerId = Guid.Parse(accountResult.CustomerId);
            newCreditCardAccount.Status = (Entities.AccountStatus)Enum.Parse(typeof(Entities.AccountStatus), accountResult.Status);
            newCreditCardAccount.AvailableBalance = (float)accountResult.AvailableBalance;
            newCreditCardAccount.CreateDate = accountResult.CreatedOn;
            newCreditCardAccount.CloseDate = accountResult.ClosingDt ?? DateTime.MaxValue;
            newCreditCardAccount.CreditCard = new Card();
            newCreditCardAccount.Type = (Entities.AccountType)Enum.Parse(typeof(Entities.AccountType), accountResult.AccountType);
            newCreditCardAccount.CreditLimit = (float)creditCardResult.CreditLimit;
            newCreditCardAccount.CreditCard.CardNumber = creditCardResult.CreditCardNum.ToString();
            newCreditCardAccount.CreditCard.Cvv = creditCardResult.CVV;
            newCreditCardAccount.CreditCard.DateOfExpiry = creditCardResult.ExpiryDt;
            newCreditCardAccount.CreditCard.DateOfIssue = creditCardResult.StartDate;
            newCreditCardAccount.CreditCard.NameOnCard = creditCardResult.Name;
            newCreditCardAccount.CreditCard.Status = (CardStatus)Enum.Parse(typeof(CardStatus), creditCardResult.Status);

            return newCreditCardAccount;
        }
    }
}
