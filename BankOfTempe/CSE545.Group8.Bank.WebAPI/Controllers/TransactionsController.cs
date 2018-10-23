using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CSE545.Group8.Bank.Entities;
using CSE545.Group8.Bank.DataAccessLayer;
using CSE545.Group8.Bank.Utilities;

namespace CSE545.Group8.Bank.WebAPI.Controllers
{
    public class TransactionsController : ApiController
    {
        // Get Single Transaction by Transaction ID
        [HttpGet]
        [ResponseType(typeof(Transaction))]
        [Authorize(Roles = "Employee,SystemManager")]
        [Route("api/transaction/{transactionId:int}")]
        public HttpResponseMessage GetTransactionByID(int transactionId)
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();

                Transaction transaction = TL.GetTransactionByID(transactionId);
                if (transaction == null) // this wont work is T is not a nullable type
                {
                    var message = "Transaction doesn't exist";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }

                return Request.CreateResponse<Transaction>(HttpStatusCode.OK, transaction);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<Transaction>))]
        [Route("api/transaction/{StartDate:DateTime}/{EndDate:DateTime}")]
        [Authorize(Roles = "Employee,SystemManager")]
        public HttpResponseMessage GetAllTransactionDateTime(DateTime startDate, DateTime endDate)
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();
                IEnumerable<Transaction> transList = TL.GetAllTransactionsByDate(startDate, endDate);
                if (transList == null || transList.Count() == 0) // this wont work is T is not a nullable type
                {
                    var message = "Transaction List doesn't exist";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                return Request.CreateResponse(HttpStatusCode.OK, transList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // Get All Transactions Per Account
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Transaction>))]
        [Authorize(Roles = "Employee,SystemManager")]
        [Route("api/transactions/account/{accountId}")]
        public HttpResponseMessage GetAllTransactionbyAccount(int accountId)
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();
                IEnumerable<Transaction> transList = null;
                transList = TL.GetAllTransactionsByAcccountID(accountId);
                if (transList == null || transList.Count() == 0) // this wont work is T is not a nullable type
                {
                    var message = "Transaction List doesn't exist";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                return Request.CreateResponse(HttpStatusCode.OK, transList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<Transaction>))]
        [Route ("api/transactions/{accountId:int}/download")]
        public HttpResponseMessage downloadTransactionHistory(int accountId)
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();
                IEnumerable<Transaction> transList = null;
                transList = TL.GetAllTransactionsByAcccountID(accountId);
                if (transList == null || transList.Count() == 0) // this wont work is T is not a nullable type
                {
                    var message = "Transaction List doesn't exist";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                else
                {
                    List<string> transactionsList = new List<string>();
                    String location = "";
                    foreach (var trans in transList)
                    {
                        var transaction = "Id: " + trans.TransactionId.ToString() + " Date: " + trans.TransactionDate.ToString() + " From Account " + trans.FromAccountId + " To Account " + trans.ToAccountId + " Type :" + trans.Type.ToString() + " Approval Status : " + trans.Approval_Status.ToString() + " Description: " + trans.Description;
                        transactionsList.Add(transaction);
                    }
                    location = PDFGenerator.generatePDF(transactionsList, null, "Transaction_History" + accountId);
                    if (location == null)
                    {
                        var message = "Failed to generate transactions history";
                        HttpError err = new HttpError(message);
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, message);
                    }
                    var notifyEmail = Request.GetOwinContext().Request.User.Identity.Name;
                    var messageBody = "Welcome to BankofTempe\n Please find the requested transaction history in the attachment";
                    EmailGenerator.Instance.sendEmail(notifyEmail, messageBody, "BankOfTempe: Transaction HIstory", location, false);

                    return Request.CreateResponse(HttpStatusCode.OK, location);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // Get All Transactions Per Account And Date Range
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Transaction>))]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        [Route("api/transaction/accounts/{accountId}/{StartDate}/{EndDate}")]
        public HttpResponseMessage GetAllTransactionsaccountdate(int accountId, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();
                IEnumerable<Transaction> transList = null;
                transList = TL.GetAllTransactionsByAcccountIDAndDate(accountId, StartDate, EndDate);
                if (transList == null || transList.Count() == 0) // this wont work is T is not a nullable type
                {
                    var message = "Transaction List doesn't exist";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                return Request.CreateResponse(HttpStatusCode.OK, transList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Transaction>))]
        [Route("api/transaction/banker/pending")]
        public HttpResponseMessage GetAllTransactionsforBankerApproval()
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();
                IEnumerable<Transaction> transList = null;
                transList = TL.GetAllTransactionsWithApprovalStatusPendingAndNotCritical();
                if (transList != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, transList);
                }
                else
                {
                    var message = "Transaction List doesn't exist";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Roles = "SystemManager")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Transaction>))]
        [Route("api/transaction/manager/pending")]
        public HttpResponseMessage GetAllTransactionsforManagerApproval()
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();
                IEnumerable<Transaction> transList = null;
                transList = TL.GetAllTransactionsWithApprovalStatusPending();
                if (transList != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, transList);
                }
                else
                {
                    var message = "Transaction List doesn't exist";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        [TwoFactorAuthorize]
        [Route("api/transaction")]
        public HttpResponseMessage InsertTransactionByID([FromBody]Transaction value)
        {
            try
            {
                float criticalAmount = 1000;
                if (!ModelState.IsValid)
                {
                    var message = "Incorrect json format";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }

                if (value.Type.Equals(TransactionType.Invalid))
                {
                    var message = "Incorrect Transaction Type";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }

                if (value.TransactionMethod.Equals(TransactionMethod.Invalid))
                {
                    var message = "Incorrect Transaction Method";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                //Input data is validated
                //Setting default values
                if (value.Amount > criticalAmount)
                {
                    value.critical = true;
                    value.Status = TransactionStatus.Processing;
                }
                else
                {
                    value.critical = false;
                    value.Status = TransactionStatus.Processed;
                }
                value.Approval_Status = TransactionApprovalStatus.Pending;
                value.TransactionDate = DateTime.Now;
                value.AccountId = value.FromAccountId;
                TransactionLayer TL = new TransactionLayer();
                int transactionId = TL.InsertTransaction(value);
                if (transactionId > 0)
                {
                    try
                    {
                        if (value.critical)
                        {
                            var notifyEmail = Request.GetOwinContext().Request.User.Identity.Name;
                            var messageBody = "Transaction initiated from account " + value.FromAccountId + " to account " + value.ToAccountId + " for the amount " + value.Amount;
                            EmailGenerator.Instance.sendEmail(notifyEmail, messageBody, "BankOfTempe: Transaction Initiated", null, false);
                        }
                        return Request.CreateResponse(HttpStatusCode.Created, transactionId);
                    }
                    catch (Exception e)
                    {
                        var message = "Transaction Initiated successfully. Failed to send email";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                    }
                }
                else
                {
                    var message = "Failed to add trasnaction";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /* Insert Transaction By Account 
        [HttpPut]
       [Route("api/transaction/{transactionid}/account")]
        public HttpResponseMessage InsertTransactionByAccount(int transactionid,[FromBody]Transaction value)
        {
            if (value == null) // this wont work is T is not a nullable type
            {
                var nullobject = new HttpResponseMessage(HttpStatusCode.BadRequest);
                throw new HttpResponseException(nullobject);
            }

            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }*/

        // Update Transcation by Transaction ID
        [HttpPost]
        [Authorize(Roles = "Employee,SystemManager")]
        [Route("api/transaction/{transactionId}")]
        public HttpResponseMessage UpdateTransactionByID(int transactionId, [FromBody]Transaction value)
        {
            try
            {
                TransactionLayer transactionLayer = new TransactionLayer();

                int restransactionId = transactionLayer.UpdateTransactionByID(transactionId, value);
                if (restransactionId != -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, transactionId);
                }
                else
                {
                    var message = "Failed to update trasnaction";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        [Route("api/transaction/approve/banker/{transactionID}")]
        public HttpResponseMessage approveTransactionBanker(int transactionId)
        {
            try
            {

                TransactionLayer TL = new TransactionLayer();
                Transaction trans = TL.GetTransactionByID(transactionId);

                if (trans != null && trans.TransactionMethod == TransactionMethod.Creditcard || trans.TransactionMethod == TransactionMethod.Paycheck && trans.Approval_Status == TransactionApprovalStatus.Pending)
                {

                    trans.Approval_Status = TransactionApprovalStatus.Approved;
                    trans.Status = TransactionStatus.Processed;
                    AccountsLayer AL = new AccountsLayer();
                    if (AccountsLayer.GetCreditCardAccountByAccountID(trans.FromAccountId) != null)
                    {
                        //Credit card is fromAccount
                        var fromCreditObj = AccountsLayer.GetCreditCardAccountByAccountID(trans.FromAccountId);
                        var toCheckAccountObj = AccountsLayer.GetCheckingAccountByAccountID(trans.ToAccountId);
                        var toSavAccountObj = AccountsLayer.GetSavingsAccountByAccountID(trans.ToAccountId);
                        TL.UpdateTransactionByID(trans.TransactionId, trans);

                        if (toCheckAccountObj != null)

                        {
                            toCheckAccountObj.AvailableBalance += trans.Amount;

                            AccountsLayer.PostCreditCardAccount(fromCreditObj.AccountId, fromCreditObj);
                            AccountsLayer.PostCheckingAccount(toCheckAccountObj.AccountId, toCheckAccountObj);

                        }
                        else if (toSavAccountObj != null)
                        {
                            toSavAccountObj.AvailableBalance += trans.Amount;

                            AccountsLayer.PostCreditCardAccount(fromCreditObj.AccountId, fromCreditObj);
                            AccountsLayer.PostSavingsAccount(toSavAccountObj.AccountId, toSavAccountObj);
                        }
                        else
                        {
                            var message = "To account is neither checking nor savings";
                            HttpError err = new HttpError(message);
                            err.Message = message;
                            return Request.CreateResponse(HttpStatusCode.NotFound, err);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);

                    }
                    else if (AccountsLayer.GetCreditCardAccountByAccountID(trans.ToAccountId) != null)
                    {
                        //Credit card is toAccount
                        var toCreditObj = AccountsLayer.GetCreditCardAccountByAccountID(trans.ToAccountId);
                        var fromCheckAccountObj = AccountsLayer.GetCheckingAccountByAccountID(trans.FromAccountId);
                        var fromSavAccountObj = AccountsLayer.GetSavingsAccountByAccountID(trans.FromAccountId);
                        TL.UpdateTransactionByID(trans.TransactionId, trans);

                        if (fromCheckAccountObj != null)

                        {
                            fromCheckAccountObj.AvailableBalance -= trans.Amount;

                            AccountsLayer.PostCreditCardAccount(toCreditObj.AccountId, toCreditObj);
                            AccountsLayer.PostCheckingAccount(fromCheckAccountObj.AccountId, fromCheckAccountObj);

                        }
                        else if (fromSavAccountObj != null)
                        {
                            fromSavAccountObj.AvailableBalance -= trans.Amount;

                            AccountsLayer.PostCreditCardAccount(toCreditObj.AccountId, toCreditObj);
                            AccountsLayer.PostSavingsAccount(fromSavAccountObj.AccountId, fromSavAccountObj);
                        }
                        else
                        {
                            var message = "To account is neither checking nor savings";
                            HttpError err = new HttpError(message);
                            err.Message = message;
                            return Request.CreateResponse(HttpStatusCode.NotFound, err);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        var message = "Invalid from and to account Ids";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);

                    }

                }

                else
                {
                    if (trans == null)
                    {
                        var message = "Failed to update trasnaction";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, err);

                    }
                    if (!trans.critical && trans.Approval_Status.Equals(TransactionApprovalStatus.Pending))
                    {
                        trans.Approval_Status = TransactionApprovalStatus.Approved;
                        var flag = TL.UpdateTransactionByID(transactionId, trans);
                        if (flag != -1)
                        {

                            var fromAccountcheck = AccountsLayer.GetCheckingAccountByAccountID(trans.FromAccountId);
                            var fromAccountsave = AccountsLayer.GetSavingsAccountByAccountID(trans.FromAccountId);
                            var toAccountcheck = AccountsLayer.GetCheckingAccountByAccountID(trans.ToAccountId);
                            var toAccountsave = AccountsLayer.GetSavingsAccountByAccountID(trans.ToAccountId);
                            var toAccountcredit = AccountsLayer.GetCreditCardAccountByAccountID(trans.ToAccountId);

                            if (fromAccountcheck != null && fromAccountcheck.AvailableBalance > trans.Amount)
                            {
                                fromAccountcheck.AvailableBalance -= trans.Amount;
                                AccountsLayer.PostCheckingAccount(trans.FromAccountId, fromAccountcheck);
                            }
                            else if (fromAccountsave != null && fromAccountsave.AvailableBalance > trans.Amount)
                            {
                                fromAccountsave.AvailableBalance -= trans.Amount;
                                AccountsLayer.PostSavingsAccount(trans.FromAccountId, fromAccountsave);
                            }
                            else
                            {
                                var errormessage = "Transaction can't be approved";
                                HttpError err = new HttpError(errormessage);
                                err.Message = errormessage;
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                            }

                            if (toAccountcheck != null)
                            {
                                toAccountcheck.AvailableBalance += trans.Amount;
                                AccountsLayer.PostCheckingAccount(trans.ToAccountId, toAccountcheck);
                            }
                            else if (toAccountsave != null)
                            {
                                toAccountsave.AvailableBalance += trans.Amount;
                                AccountsLayer.PostSavingsAccount(trans.ToAccountId, toAccountsave);
                            }
                            else if (toAccountcredit != null)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                var errormessage = "Transaction can't be approved";
                                HttpError err = new HttpError(errormessage);
                                err.Message = errormessage;
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                            }
                            return Request.CreateResponse(HttpStatusCode.OK);

                        }
                        else
                        {
                            var errormessage = "Transaction can't be approved";
                            HttpError err = new HttpError(errormessage);
                            err.Message = errormessage;
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, err);

                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SystemManager")]
        [Route("api/transaction/approve/manager/{transactionID}")]
        public HttpResponseMessage ApproveTransactionManager(int transactionId)
        {
            try
            {
                TransactionLayer TL = new TransactionLayer();
                Transaction trans = TL.GetTransactionByID(transactionId);

                if (trans != null && trans.TransactionMethod == TransactionMethod.Creditcard && trans.TransactionMethod == TransactionMethod.Paycheck && trans.Approval_Status == TransactionApprovalStatus.Pending)
                {
                    trans.Approval_Status = TransactionApprovalStatus.Approved;
                    trans.Status = TransactionStatus.Processed;
                    AccountsLayer AL = new AccountsLayer();
                    if (AccountsLayer.GetCreditCardAccountByAccountID(trans.FromAccountId) != null)
                    {
                        //Credit card is fromAccount
                        var fromCreditObj = AccountsLayer.GetCreditCardAccountByAccountID(trans.FromAccountId);
                        var toCheckAccountObj = AccountsLayer.GetCheckingAccountByAccountID(trans.ToAccountId);
                        var toSavAccountObj = AccountsLayer.GetSavingsAccountByAccountID(trans.ToAccountId);
                        TL.UpdateTransactionByID(trans.TransactionId, trans);

                        if (toCheckAccountObj != null)

                        {
                            toCheckAccountObj.AvailableBalance += trans.Amount;

                            AccountsLayer.PostCreditCardAccount(fromCreditObj.AccountId, fromCreditObj);
                            AccountsLayer.PostCheckingAccount(toCheckAccountObj.AccountId, toCheckAccountObj);

                        }
                        else if (toSavAccountObj != null)
                        {
                            toSavAccountObj.AvailableBalance += trans.Amount;

                            AccountsLayer.PostCreditCardAccount(fromCreditObj.AccountId, fromCreditObj);
                            AccountsLayer.PostSavingsAccount(toSavAccountObj.AccountId, toSavAccountObj);
                        }
                        else
                        {
                            var message = "To account is neither checking nor savings";
                            HttpError err = new HttpError(message);
                            err.Message = message;
                            return Request.CreateResponse(HttpStatusCode.NotFound, err);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else if (AccountsLayer.GetCreditCardAccountByAccountID(trans.ToAccountId) != null)
                    {
                        //Credit card is toAccount
                        var toCreditObj = AccountsLayer.GetCreditCardAccountByAccountID(trans.ToAccountId);
                        var fromCheckAccountObj = AccountsLayer.GetCheckingAccountByAccountID(trans.FromAccountId);
                        var fromSavAccountObj = AccountsLayer.GetSavingsAccountByAccountID(trans.FromAccountId);
                        TL.UpdateTransactionByID(trans.TransactionId, trans);

                        if (fromCheckAccountObj != null)

                        {
                            fromCheckAccountObj.AvailableBalance -= trans.Amount;

                            AccountsLayer.PostCreditCardAccount(toCreditObj.AccountId, toCreditObj);
                            AccountsLayer.PostCheckingAccount(fromCheckAccountObj.AccountId, fromCheckAccountObj);
                        }
                        else if (fromSavAccountObj != null)
                        {
                            fromSavAccountObj.AvailableBalance -= trans.Amount;

                            AccountsLayer.PostCreditCardAccount(toCreditObj.AccountId, toCreditObj);
                            AccountsLayer.PostSavingsAccount(fromSavAccountObj.AccountId, fromSavAccountObj);
                        }
                        else
                        {
                            var message = "To account is neither checking nor savings";
                            HttpError err = new HttpError(message);
                            err.Message = message;
                            return Request.CreateResponse(HttpStatusCode.NotFound, err);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        var message = "Invalid from and to account Ids";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }

                }
                else
                {
                    if (trans == null)
                    {
                        var message = "Failed to update trasnaction";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                    }
                    if (trans.Approval_Status.Equals(TransactionApprovalStatus.Pending))
                    {
                        trans.Approval_Status = TransactionApprovalStatus.Approved;
                        var flag = TL.UpdateTransactionByID(transactionId, trans);
                        if (flag != 1)
                        {

                            var fromAccount = AccountsLayer.GetCheckingAccountByAccountID(trans.FromAccountId);
                            fromAccount.AvailableBalance -= trans.Amount;
                            AccountsLayer.PostCheckingAccount(fromAccount.AccountId, fromAccount);

                            var toAccount = AccountsLayer.GetCheckingAccountByAccountID(trans.ToAccountId);
                            toAccount.AvailableBalance += trans.Amount;
                            AccountsLayer.PostCheckingAccount(toAccount.AccountId, toAccount);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /*  // Update Transaction By Accout ID
          [HttpPost]
          [Route("api/transaction/account/{transactionId}/account")]
          public HttpResponseMessage UpdateTransactionByAccount(int transactionId, [FromBody]Transaction value)
          {
              if (value == null) // this wont work is T is not a nullable type
              {
                  var nullobject = new HttpResponseMessage(HttpStatusCode.BadRequest);
                  throw new HttpResponseException(nullobject);
              }
              TransactionLayer TL = new TransactionLayer();
              TL.UpdateTransactionByID(transactionId, value);
              return Request.CreateResponse(HttpStatusCode.Created, value);
          }*/


        // Delete Transaction By ID
        /*        [HttpDelete]
                [Route("api/transactions/{transactionId}")]
                public HttpResponseMessage DeleteTransactionByID(int transactionId)
                {
                    TransactionLayer TL = new TransactionLayer();
                    TL.deleteTransactionByID(transactionId);
                    return Request.CreateResponse(HttpStatusCode.Accepted, transactionId);
                }*/


        /* // Delete Transaction By Account
         [HttpDelete]
         [Route("api/transaction/account/{transactionId}")]
         public HttpResponseMessage DeleteTransactionByAccount(int accountId)
         {
            //TODO:
             //<Transaction> Tlist = Fetch All Transaction on DB
             //var T = Tlist.FirstOrDefault(x => x.history.AccountId == Aid);
             // Tlist.Remove(t)
             //Update List in DB
             return Request.CreateResponse(HttpStatusCode.NotImplemented);
         }*/


        // Delete Transactions By Date Range
        /*      [HttpDelete]
            [Route("api/transaction/{StartDate}/{EndDate}")]
            public HttpResponseMessage DeleteAllTransaction(DateTime StartDate, DateTime EndDate)
            {

                TransactionLayer TL = new TransactionLayer();
                TL.deleteAllTransactionByDate(StartDate, EndDate);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            } */


        // Delete Multiple Transactions per Account
        /*[HttpDelete]
        [Route("api/transaction/accounts/{accountId}")]
        public HttpResponseMessage DeleteAllTransactionbyAccount(int accountId)
        {
            TransactionLayer TL = new TransactionLayer();
            TL.deleteAllTransactionByAccountID(accountId);
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        } */


        // Delete Multiple Transaction Per Account and Date Range
        /*[HttpDelete]
        [Route("api/transaction/accounts/{accountId}/{Startdate}/{EndDate}")]
        public HttpResponseMessage DeleteAllTransaction(int accountId, DateTime StartDate, DateTime EndDate)
        {
            TransactionLayer TL = new TransactionLayer();
            TL.deleteAllTransactionByDateAndAccountID(accountId, StartDate, EndDate);
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }*/
    }
}
