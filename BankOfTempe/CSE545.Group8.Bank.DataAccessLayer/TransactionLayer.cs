using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankofasuContext;
using System.Transactions;

namespace CSE545.Group8.Bank.DataAccessLayer
{
    public class TransactionLayer
    {
        public Entities.Transaction GetTransactionByID(int transactionID)
        {
            var transaction = new Entities.Transaction();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var results = from trans in context.Transactions
                                  where trans.TransactionId == transactionID
                                  select trans;
                    if (results.Count() != 0)
                    {
                        var result = results.FirstOrDefault();

                        transaction = ParseTransaction(result);
                    }
                    else
                        return null;

                }
                scope.Complete();
                return transaction;
            }

        }

        public IEnumerable<Entities.Transaction> GetAllTransactionsWithApprovalStatusPending()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var transaction = new List<Entities.Transaction>();
                using (var context = new BankofasuDataContext())
                {
                    //(Entities.TransactionMethod)Enum.Parse(typeof(Entities.TransactionMethod), result.TransactionMethod)

                        var results = from trans in context.Transactions
                                      where trans.ApprovalStatus == Entities.TransactionApprovalStatus.Pending.ToString()
                                      select trans;

                        if (results.Count() != 0)
                        {
                            foreach (var result in results)
                            {
                                var ind_transaction = new Entities.Transaction();
                                ind_transaction = ParseTransaction(result);
                                transaction.Add(ind_transaction);
                            }
                        }
                        else
                            return null;
                   
                }

                scope.Complete();
                return transaction;

            }


        }

        public IEnumerable<Entities.Transaction> GetAllTransactionsWithApprovalStatusPendingandCritical(int accountID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var transaction = new List<Entities.Transaction>();
                using (var context = new BankofasuDataContext())
                {
                    var accountin = from accounts in context.Accounts
                                    where accounts.AccountId == accountID
                                    select accounts;

                    if (accountin.Count() != 0)
                    {
                        var results = from trans in context.Transactions
                                      where trans.AccountId == accountID && trans.ApprovalStatus == Entities.TransactionApprovalStatus.Pending.ToString() && trans.Critical == true
                                      select trans;

                        if (results.Count() != 0)
                        {
                            foreach (var result in results)
                            {
                                var ind_transaction = new Entities.Transaction();
                                ind_transaction = ParseTransaction(result);
                                transaction.Add(ind_transaction);
                            }
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }

                scope.Complete();
                return transaction;

            }


        }

        public IEnumerable<Entities.Transaction> GetAllTransactionsWithApprovalStatusPendingAndNotCritical()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var transaction = new List<Entities.Transaction>();
                using (var context = new BankofasuDataContext())
                {

                        var results = from trans in context.Transactions
                                      where trans.ApprovalStatus == Entities.TransactionApprovalStatus.Pending.ToString() && trans.Critical == false
                                      select trans;

                        if (results.Count() != 0)
                        { 
                            foreach (var result in results)
                            {
                                var ind_transaction = new Entities.Transaction();
                                ind_transaction = ParseTransaction(result);
                                transaction.Add(ind_transaction);
                            }
                        }
                        else
                            return null;

                }

                scope.Complete();
                return transaction;

            }


        }

        public int deleteTransactionByID(int transactionID)
        {
            var transaction = new Entities.Transaction();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var results = from trans in context.Transactions
                                  where trans.TransactionId == transactionID
                                  select trans;

                    if (results.Count() != 0)
                    {
                        var result = results.FirstOrDefault();
                        context.Transactions.DeleteOnSubmit(result);
                        context.SubmitChanges();
                    }
                    else
                        return -1;
                }
                scope.Complete();
                return transactionID;
            }
          
        }

        public void deleteAllTransactionByDate(DateTime StartDate, DateTime EndDate)
        {
            var transaction = new Entities.Transaction();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var results = from trans in context.Transactions
                                  where trans.TransactionDate >= StartDate && trans.TransactionDate <= EndDate
                                  select trans;
                  
                    foreach (var t in results)
                    {
                        context.Transactions.DeleteOnSubmit(t);
                    }
                    context.SubmitChanges();
                }
                scope.Complete();
            }
           
        }

        public int deleteAllTransactionByDateAndAccountID(int accountID, DateTime StartDate, DateTime EndDate)
        {
            var transaction = new Entities.Transaction();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var accountin = from accounts in context.Accounts
                                    where accounts.AccountId == accountID
                                    select accounts;

                    if (accountin.Count() != 0)
                    {
                        var results = from trans in context.Transactions
                                      where trans.TransactionDate >= StartDate && trans.TransactionDate <= EndDate && trans.AccountId == accountID
                                      select trans;

                        foreach (var t in results)
                        {
                            context.Transactions.DeleteOnSubmit(t);
                        }
                        context.SubmitChanges();
                    }
                    else
                        return -1;
                }
                scope.Complete();
                return accountID;
            }
       
        }

        public int deleteAllTransactionByAccountID(int accountID)
        {
            var transaction = new Entities.Transaction();
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var accountin = from accounts in context.Accounts
                                    where accounts.AccountId == accountID
                                    select accounts;
                    if (accountin.Count() != 0)
                    {
                        var results = from trans in context.Transactions
                                      where trans.AccountId == accountID
                                      select trans;
                        foreach (var t in results)
                        {
                            context.Transactions.DeleteOnSubmit(t);
                        }
                        context.SubmitChanges();
                    }
                    else
                        return -1;
                }
                scope.Complete();
                return accountID;
            }
           
        }
        public Entities.Transaction GetTransactionByAccountID(int AccountID)
        {
            var transaction = new Entities.Transaction();
            using (var context = new BankofasuDataContext())
            {
                var accountin = from accounts in context.Accounts
                                where accounts.AccountId == AccountID
                                select accounts;
                if (accountin.Count() != 0)
                {
                    var results = from trans in context.Transactions
                                    where trans.AccountId == AccountID
                                    select trans;
                    var result = results.FirstOrDefault();

                    transaction = ParseTransaction(result);
                    return transaction;
                }
                return null;
            }

        }

        private Entities.Transaction ParseTransaction(BankofasuContext.Transaction result)
        {
            if(result != null)
            { 
                return new Entities.Transaction
                {
                    AccountId = (int)result.AccountId,
                    Amount = (float)result.Amount,
                    Description = result.Description,
                    FromAccountId = (int)result.TransferFrom,
                    Status = (Entities.TransactionStatus)Enum.Parse(typeof(Entities.TransactionStatus), result.Status),
                    ToAccountId = (int)result.TransferTo,
                    TransactionId = (int)result.TransactionId,
                    TransactionMethod = (Entities.TransactionMethod)Enum.Parse(typeof(Entities.TransactionMethod),result.TransactionMethod),
                    Type = (Entities.TransactionType)Enum.Parse(typeof(Entities.TransactionType), result.TransactionType),
                    TransactionDate = result.TransactionDate
                };
            }
            return null;
        }
        
        public IEnumerable<Entities.Transaction> GetAllTransactionsByAcccountIDAndDate(int accountID, DateTime startDate, DateTime endDate)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var transaction = new List<Entities.Transaction>();
                using (var context = new BankofasuDataContext())
                {
                    var accountin = from accounts in context.Accounts
                                    where accounts.AccountId == accountID
                                    select accounts;
                    if (accountin.Count() != 0)
                    {
                        var results = from trans in context.Transactions
                                      where trans.AccountId == accountID && trans.TransactionDate >= startDate && trans.TransactionDate <= endDate
                                      select trans;
                        foreach (var result in results)
                        {
                            var ind_transaction = new Entities.Transaction();
                            ind_transaction = ParseTransaction(result);
                            transaction.Add(ind_transaction);
                        }
                    }
                    else
                        return null;
                }
                scope.Complete();
                return transaction;

            }
           

        }

        public IEnumerable<Entities.Transaction> GetAllTransactionsByDate(DateTime startDate, DateTime endDate)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var transaction = new List<Entities.Transaction>();
                using (var context = new BankofasuDataContext())
                {
                    var results = from trans in context.Transactions
                                  where trans.TransactionDate >= startDate && trans.TransactionDate <= endDate
                                  select trans;
                    foreach (var result in results)
                    {
                        var ind_transaction = new Entities.Transaction();
                        ind_transaction = ParseTransaction(result);
                        transaction.Add(ind_transaction);
                    }

                }
                scope.Complete();
                return transaction;
            }
              
        }

        public IEnumerable<Entities.Transaction> GetAllTransactionsByAcccountID(int accountID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var transaction = new List<Entities.Transaction>();
                using (var context = new BankofasuDataContext())
                {
                    var accountin = from accounts in context.Accounts
                                    where accounts.AccountId == accountID
                                    select accounts;
                    if (accountin.Count() != 0)
                    {
                        var results = from trans in context.Transactions
                                      where trans.TransferFrom == accountID || trans.TransferTo == accountID
                                      select trans;
                        foreach (var result in results)
                        {
                            var ind_transaction = new Entities.Transaction();
                            ind_transaction = ParseTransaction(result);
                            transaction.Add(ind_transaction);
                        }
                    }
                    else
                        return null;
                }
                scope.Complete();
                return transaction;
            }
          
        }

        public int InsertTransaction(Entities.Transaction trans)
        {
            if (trans.TransactionMethod == Entities.TransactionMethod.Creditcard)
            {
                using (var context = new BankofasuDataContext())
                {
                    var get_creditnum = from CreditcardDetail in context.CreditcardDetails
                                        where CreditcardDetail.AccountId == trans.AccountId
                                        select CreditcardDetail.CreditCardNum;

                    var check_if_numexists = from CreditcardTransaction in context.CreditcardTransactions
                                             where CreditcardTransaction.CreditCardNum == get_creditnum.FirstOrDefault()
                                             select CreditcardTransaction;

                    if (check_if_numexists.Count() == 0)
                    {
                        DateTime today = DateTime.Today;
                        DateTime endOfMonth = new DateTime(today.Year,
                                                           today.Month,
                                                           DateTime.DaysInMonth(today.Year,
                                                                                today.Month));
                        var creditBalance = 1500;
                        var credit_number = get_creditnum.FirstOrDefault();
                        var creditcard_transactions = new BankofasuContext.CreditcardTransaction
                        {
                            CreditCardNum = credit_number,
                            CreditBalance = creditBalance,
                            LastPaymentDt = null,
                            LastPaymentAmt = 0,
                            NextPaymentDt = endOfMonth.AddDays(15),
                            MonthlyStatementBalance = 0,
                            NextPaymentDue = 0,
                            MinPayDue = 0,
                            LastUpdatedDt = DateTime.Now,
                            MonthlyStatementEndDt = endOfMonth
                        };
                        context.CreditcardTransactions.InsertOnSubmit(creditcard_transactions);
                        context.SubmitChanges();
                    }
                }
            }
            using (TransactionScope scope = new TransactionScope())
            {
                var transaction = new BankofasuContext.Transaction
                {
                    AccountId = trans.AccountId,
                    Amount = (decimal)trans.Amount,
                    Description = trans.Description,
                    TransferFrom = (int)trans.FromAccountId,
                    Status = trans.Status.ToString(),
                    TransferTo = (int)trans.ToAccountId,
                    TransactionId = (int)trans.TransactionId,
                    TransactionType = trans.Type.ToString(),
                    TransactionDate = DateTime.Now,
                    TransactionMethod = trans.TransactionMethod.ToString(),
                    ApprovalStatus = trans.Approval_Status.ToString(),
                    Critical = trans.critical

                };

                using (var context = new BankofasuDataContext())
                {
                    var accountin = from accounts in context.Accounts
                                    where accounts.AccountId == trans.FromAccountId
                                    select accounts;
                    var accountout = from accounts in context.Accounts
                                     where accounts.AccountId == trans.ToAccountId
                                     select accounts;

                    if (accountin.Count() > 0 && accountout.Count() > 0 && transaction.Amount >= 0 && transaction.Description != null && (trans.Status == Entities.TransactionStatus.Blocked || trans.Status == Entities.TransactionStatus.Processed || trans.Status == Entities.TransactionStatus.Processing || trans.Status == Entities.TransactionStatus.Reverted)  && (trans.Type == Entities.TransactionType.Cash || trans.Type == Entities.TransactionType.InterBankTransfer || trans.Type == Entities.TransactionType.InternalTransfer || trans.Type == Entities.TransactionType.IntraBankTransfer || trans.Type == Entities.TransactionType.Payment) && transaction.TransactionDate != null && (trans.TransactionMethod == Entities.TransactionMethod.Creditcard || trans.TransactionMethod == Entities.TransactionMethod.Debitcard || trans.TransactionMethod == Entities.TransactionMethod.Paycheck) && (trans.Approval_Status == Entities.TransactionApprovalStatus.Approved || trans.Approval_Status == Entities.TransactionApprovalStatus.BankerApproved || trans.Approval_Status == Entities.TransactionApprovalStatus.Declined || trans.Approval_Status == Entities.TransactionApprovalStatus.Pending || trans.Approval_Status == Entities.TransactionApprovalStatus.ReceiverApproved) && (transaction.Critical == false || transaction.Critical == true))
                    {
                        context.Transactions.InsertOnSubmit(transaction);
                        context.SubmitChanges();
                    }
                    else
                        return -1;
                }

                scope.Complete();
                return (int)transaction.TransactionId;
            }

        }

        public int UpdateTransactionByID(int transactionID, Entities.Transaction trans)
        {
            if (trans.TransactionMethod != Entities.TransactionMethod.Creditcard)
            {
                using (var context = new BankofasuDataContext())
                {
                    var toaccount = from CreditcardDetail in context.CreditcardDetails
                                    where CreditcardDetail.AccountId == trans.ToAccountId
                                    select CreditcardDetail.CreditCardNum;
                    var credit_number = toaccount.FirstOrDefault();
                    if (credit_number != 0)
                    {
                        var credit_transactions = from CreditcardTransaction in context.CreditcardTransactions
                                                  where CreditcardTransaction.CreditCardNum == credit_number
                                                  select CreditcardTransaction;
                        var credit_transaction = credit_transactions.FirstOrDefault();
                        var credit_balance = 0;
                        var payment_due = 0;
                        var min_due = 0;
                        var next_payment_due = 0;
                        DateTime today = DateTime.Today;
                        DateTime endOfMonth = new DateTime(today.Year,
                                                           today.Month,
                                                           DateTime.DaysInMonth(today.Year,
                                                                                today.Month));
                        if (DateTime.Now == endOfMonth)
                        {
                            endOfMonth = endOfMonth.AddDays(30);
                        }
                        if (credit_transaction.CreditBalance + (decimal)trans.Amount == 1500)
                        {
                            credit_balance = 1500;
                        }
                        else
                        {
                            credit_balance = (int)(credit_transaction.CreditBalance + (decimal)trans.Amount);
                            payment_due = 1500 - credit_balance;
                            next_payment_due = (int)credit_transaction.NextPaymentDue + payment_due;
                            if (payment_due >= 35)
                            {
                                min_due = 35;
                                payment_due += (int)(0.1 * payment_due);
                            }
                            else
                            {
                                min_due = payment_due;
                            }
                        }

                        //var creditcard_transaction = new BankofasuContext.CreditcardTransaction
                        //{
                        credit_transaction.CreditBalance = credit_balance;
                        credit_transaction.LastPaymentDt = DateTime.Now;
                        credit_transaction.LastPaymentAmt = (decimal)trans.Amount;
                        credit_transaction.NextPaymentDt = credit_transaction.NextPaymentDt.AddDays(30);
                        credit_transaction.MonthlyStatementBalance = min_due;
                        credit_transaction.NextPaymentDue = next_payment_due;
                        credit_transaction.MinPayDue = min_due;
                        credit_transaction.LastUpdatedDt = DateTime.Now;
                        credit_transaction.MonthlyStatementEndDt = endOfMonth;
                       // };
                        context.SubmitChanges();
                    }
                    else return -1;
                }
            }
            else
            {
                using (var context = new BankofasuDataContext())
                {
                    var fromaccount = from CreditcardDetail in context.CreditcardDetails
                                      where CreditcardDetail.AccountId == trans.FromAccountId
                                      select CreditcardDetail.CreditCardNum;
                    var credit_number = fromaccount.FirstOrDefault();
                    if (credit_number != 0)
                    {
                        var credit_transactions = from CreditcardTransaction in context.CreditcardTransactions
                                                  where CreditcardTransaction.CreditCardNum == credit_number
                                                  select CreditcardTransaction;
                        var credit_transaction = credit_transactions.FirstOrDefault();
                        var credit_balance = credit_transaction.CreditBalance - (decimal)trans.Amount;
                        //var min_due = 0;
                        DateTime today = DateTime.Today;
                        DateTime endOfMonth = new DateTime(today.Year,
                                                           today.Month,
                                                           DateTime.DaysInMonth(today.Year,
                                                                                today.Month));
                        var next_payment_due = 0;
                        if (today > credit_transaction.MonthlyStatementEndDt)
                        {
                            next_payment_due = (int)(credit_transaction.NextPaymentDue + (decimal)trans.Amount);
                        }
                        else
                        {
                            next_payment_due = (int)credit_transaction.NextPaymentDue;
                        }
                        if (DateTime.Now == endOfMonth)
                        {
                            endOfMonth = endOfMonth.AddDays(30);
                        }


                        //var creditcard_transaction = new BankofasuContext.CreditcardTransaction
                        //{
                        //creditcard_transaction.CreditCardNum = credit_transaction.CreditCardNum,
                        credit_transaction.CreditBalance = credit_balance;
                        credit_transaction.MonthlyStatementBalance = credit_balance;
                        credit_transaction.NextPaymentDue = next_payment_due;
                        credit_transaction.LastUpdatedDt = DateTime.Now;
                        //};
                        context.SubmitChanges();
                    }
                    else return -1;
                }
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var accountin = from accounts in context.Accounts
                                    where accounts.AccountId == trans.AccountId
                                    select accounts;
                    var accountout = from accounts in context.Accounts
                                     where accounts.AccountId == trans.ToAccountId
                                     select accounts;
                    var results = from transt in context.Transactions
                                  where transt.TransactionId == transactionID
                                  select transt;
                    var result = results.FirstOrDefault();

                    if (results!= null && accountin.Count() != 0 && trans.Amount >= 0 && trans.Description != null && (trans.Status == Entities.TransactionStatus.Blocked || trans.Status == Entities.TransactionStatus.Processed || trans.Status == Entities.TransactionStatus.Processing || trans.Status == Entities.TransactionStatus.Reverted) && accountout != null && (trans.Type == Entities.TransactionType.Cash || trans.Type == Entities.TransactionType.InterBankTransfer || trans.Type == Entities.TransactionType.InternalTransfer || trans.Type == Entities.TransactionType.IntraBankTransfer || trans.Type == Entities.TransactionType.Payment) && trans.TransactionDate != null && (trans.TransactionMethod == Entities.TransactionMethod.Creditcard || trans.TransactionMethod == Entities.TransactionMethod.Debitcard || trans.TransactionMethod == Entities.TransactionMethod.Paycheck) && (trans.Approval_Status == Entities.TransactionApprovalStatus.Approved || trans.Approval_Status == Entities.TransactionApprovalStatus.BankerApproved || trans.Approval_Status == Entities.TransactionApprovalStatus.Declined || trans.Approval_Status == Entities.TransactionApprovalStatus.Pending || trans.Approval_Status == Entities.TransactionApprovalStatus.ReceiverApproved) && (trans.critical == false || trans.critical == true))
                    {
                        result.AccountId = (int)trans.AccountId;
                        result.Amount = (decimal)trans.Amount;
                        result.Description = trans.Description;
                        result.TransferFrom = (int)trans.FromAccountId;
                        result.Status = trans.Status.ToString();
                        result.TransferTo = (int)trans.ToAccountId;
                        result.TransactionId = (int)trans.TransactionId;
                        result.TransactionType = trans.Type.ToString();
                        result.TransactionDate = DateTime.Now;
                        result.TransactionMethod = trans.TransactionMethod.ToString();
                        result.ApprovalStatus = trans.Approval_Status.ToString();
                        result.Critical = trans.critical;
                        context.SubmitChanges();
                    }
                    else
                        return -1;
                }

                scope.Complete();
                return transactionID;
            }

        }

       /* public int UpdateTransactionByStatus(int transactionID, Entities.TransactionApprovalStatus stat)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (var context = new BankofasuDataContext())
                {
                    var results = from trans in context.Transactions
                                  where trans.TransactionId == transactionID
                                  select trans;
                    var result = results.FirstOrDefault();


                    if (result != null)
                    {
                        result.Status = stat.ToString();
                        context.SubmitChanges();
                    }
                    else
                        return -1;
                }

                scope.Complete();
                return transactionID;
            }

        }*/
    }
}
