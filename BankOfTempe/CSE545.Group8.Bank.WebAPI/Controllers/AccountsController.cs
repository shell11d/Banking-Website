using CSE545.Group8.Bank.Entities;
using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CSE545.Group8.Bank.DataAccessLayer;
using CSE545.Group8.Bank.Validators;


namespace CSE545.Group8.Bank.WebAPI.Controllers
{
    public class AccountsController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(List<Account>))]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        [Route("api/users/{userId:guid}/accounts")]
        public HttpResponseMessage GetAllAccountsByUserId(Guid userId)
        {
            try
            {
                var accounts = new List<Account>();

                var checkingAccount = AccountsLayer.GetCheckingAccountByUserID(userId);
                if(checkingAccount != null){
                accounts.Add(checkingAccount);
                }
                var savingsAccount = AccountsLayer.GetSavingsAccountByUserID(userId);
                if(savingsAccount != null){
                accounts.Add(savingsAccount);
                }
                
                if (accounts.Capacity > 0)
                {
                    return Request.CreateResponse<List<Account>>(HttpStatusCode.OK, accounts);
                }
                else
                {
                    var message = string.Format("No accounts exist for this user", userId);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/creditCard/{userid}")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getCreditCardAccountbyUserId(Guid userid)
        {
            try
            {
                var message = "";
                var creditcardAccount = AccountsLayer.GetCreditCardAccountByUserID(userid);
                if (creditcardAccount != null)
                {
                    return Request.CreateResponse<CreditCardAccount>(HttpStatusCode.OK, creditcardAccount);
                }
                else
                {
                    message = "CreditCard account doesn't exist for this user";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        [Route("api/accounts/checking/{accountId:int}")]
        public HttpResponseMessage getCheckingAccountbyId(int accountId)
        {
            try
            {
                var checkingAccount = AccountsLayer.GetCheckingAccountByAccountID(accountId);
                if (checkingAccount != null)
                {
                    return Request.CreateResponse<CheckingAccount>(HttpStatusCode.OK, checkingAccount);
                }
                else
                {
                    var message = string.Format("No checking account exist for this user", accountId);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/checking/userEmail")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getCheckingAccountbyEmailAddress(string userEmail)
        {
            try
            {
                var sanitizer = new Sanitizer();
                var message = "";
                if (!sanitizer.IsValidEmail(userEmail))
                {
                    message = string.Format("Email address format invalid", userEmail);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                var individualUser = UserLayer.GetIndividualUserByEmailID(userEmail);
                if (individualUser != null)
                {
                    var checkingAccount = AccountsLayer.GetCheckingAccountByUserID(individualUser.UserId);
                    if (checkingAccount != null)
                    {
                        return Request.CreateResponse<CheckingAccount>(HttpStatusCode.OK, checkingAccount);
                    }
                    else
                    {
                        message = "Checking account doesn't exist for this user";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                }
                else
                {
                    message = string.Format("User doesn't exist", userEmail);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/checking/phoneNumber")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getCheckingAccountbyPhoneNumber(string phone)
        {
            try
            {
                var sanitizer = new Sanitizer();
                var message = "";
                if (!sanitizer.validatePhoneNumber(phone))
                {
                    message = string.Format("Phone number not valid", phone);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                var user = UserLayer.GetUserByPhoneNumber(phone);
                if (user != null)
                {

                    var checkingAccount = AccountsLayer.GetCheckingAccountByUserID(user.UserId);
                    if (checkingAccount != null)
                    {
                        return Request.CreateResponse<CheckingAccount>(HttpStatusCode.OK, checkingAccount);
                    }
                    else
                    {
                        message = "Checking account doesn't exist for this user";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                }
                else
                {
                    message = string.Format("User doesn't exist", phone);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/savings/{accountId:int}")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getSavingsAccountbyId(int accountId)
        {
            try
            {
                var savingsAccount = AccountsLayer.GetSavingsAccountByAccountID(accountId);
                if (savingsAccount != null)
                {
                    return Request.CreateResponse<SavingsAccount>(HttpStatusCode.OK, savingsAccount);
                }
                else
                {
                    var message = string.Format("No savings account exist for this user", accountId);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/savings/userEmail")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getSavingsAccountbyEmailAddress(string userEmail)
        {
            try
            {
                var sanitizer = new Sanitizer();

                var message = "";
                if (!sanitizer.IsValidEmail(userEmail))
                {
                    message = string.Format("Email address format invalid", userEmail);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                var individualUser = UserLayer.GetIndividualUserByEmailID(userEmail);
                if (individualUser != null)
                {
                    var savingsAccount = AccountsLayer.GetSavingsAccountByUserID(individualUser.UserId);
                    if (savingsAccount != null)
                    {
                        return Request.CreateResponse<SavingsAccount>(HttpStatusCode.OK, savingsAccount);
                    }
                    else
                    {
                        message = "savings account doesn't exist for this user";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                }
                else
                {
                    message = string.Format("User doesn't exist", userEmail);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/savings/phoneNumber")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getSavingsAccountbyPhoneNumber(string phone)
        {
            try
            {
                var sanitizer = new Sanitizer();
                var message = "";
                if (!sanitizer.validatePhoneNumber(phone))
                {
                    message = string.Format("Phone number not valid", phone);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                var user = UserLayer.GetUserByPhoneNumber(phone);
                if (user != null)
                {

                    var savingsAccount = AccountsLayer.GetSavingsAccountByUserID(user.UserId);
                    if (savingsAccount != null)
                    {
                        return Request.CreateResponse<SavingsAccount>(HttpStatusCode.OK, savingsAccount);
                    }
                    else
                    {
                        message = "savings account doesn't exist for this user";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }

                }
                else
                {
                    message = string.Format("User doesn't exist", phone);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);

                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }



        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/creditcard/{accountId:int}")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getCreditCardAccountbyId(int accountId)
        {
            try
            {
                var creditCardAccount = AccountsLayer.GetCreditCardAccountByAccountID(accountId);
                if (creditCardAccount != null)
                {
                    return Request.CreateResponse<CreditCardAccount>(HttpStatusCode.OK, creditCardAccount);

                }
                else
                {
                    var message = string.Format("No creditcard account exist for this user", accountId);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/creditcard/userEmail")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getCreditCardAccountbyEmailAddress(string userEmail)
        {
            try
            {
                var sanitizer = new Sanitizer();
                var message = "";
                if (!sanitizer.IsValidEmail(userEmail))
                {
                    message = string.Format("Email address format invalid", userEmail);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                var individualUser = UserLayer.GetIndividualUserByEmailID(userEmail);
                if (individualUser != null)
                {

                    var creditcardAccount = AccountsLayer.GetCreditCardAccountByUserID(individualUser.UserId);
                    if (creditcardAccount != null)
                    {
                        return Request.CreateResponse<CreditCardAccount>(HttpStatusCode.OK, creditcardAccount);
                    }
                    else
                    {
                        message = "CreditCard account doesn't exist for this user";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }

                }
                else
                {
                    message = string.Format("User doesn't exist", userEmail);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }


            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        [ResponseType(typeof(Account))]
        [Route("api/accounts/creditCard/phoneNumber")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage getCreditCardAccountbyPhoneNumber(string phone)
        {
            try
            {
                var sanitizer = new Sanitizer();
                var message = "";
                if (!sanitizer.validatePhoneNumber(phone))
                {
                    message = string.Format("Phone number not valid", phone);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                var user = UserLayer.GetUserByPhoneNumber(phone);
                if (user != null)
                {

                    var creditcardAccount = AccountsLayer.GetCreditCardAccountByUserID(user.UserId);
                    if (creditcardAccount != null)
                    {
                        return Request.CreateResponse<CreditCardAccount>(HttpStatusCode.OK, creditcardAccount);
                    }
                    else
                    {
                        message = "CreditCard account doesn't exist for this user";
                        HttpError err = new HttpError(message);
                        err.Message = message;
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }

                }
                else
                {
                    message = string.Format("User doesn't exist", phone);
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /*[HttpGet]
        [ResponseType(typeof(List<Account>))]
        [Route("api/accounts")]
        public HttpResponseMessage getAccountsByAccountId([FromBody]List<int> accountIds)
        {
            return Request.CreateResponse<List<Account>>(HttpStatusCode.NotImplemented, new List<Account> { });
        }*/



        [HttpPut]
        [Route("api/users/accounts/checking")]
        [Authorize(Roles = "Employee,SystemManager")]
        public HttpResponseMessage CreateCheckingAccount([FromBody]CheckingAccount account)
        {
            try
            {
                bool invalidModel = false;
                var message = "";
                if (!ModelState.IsValid)
                {
                    invalidModel = true;
                    message = "Incorrect json format";
                }
                if (!account.Type.Equals(AccountType.Checking))
                {
                    invalidModel = true;
                    message = "Invalid account type";
                }

                if (invalidModel)
                {
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }


                var accountId = AccountsLayer.PutCheckingAccount(account);
                if (accountId != -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, accountId);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/users/accounts/savings")]
        [Authorize(Roles = "Employee,SystemManager")]

        public HttpResponseMessage CreateSavingsAccount([FromBody]SavingsAccount account)
        {
            try
            {
                bool invalidModel = false;
                var message = "";
                if (!ModelState.IsValid)
                {
                    invalidModel = true;
                    message = "Incorrect json format";
                }
                if (!account.Type.Equals(AccountType.Savings))
                {
                    invalidModel = true;
                    message = "Invalid account type";
                }

                if (invalidModel)
                {
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }

                var accountId = AccountsLayer.PutSavingsAccount(account);
                if (accountId != -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, accountId);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/users/accounts/creditcard")]
        //[Authorize(Roles = "Employee,SystemManager")]

        public HttpResponseMessage CreateCreditCardAccount([FromBody]CreditCardAccount account)
        {
            try
            {
                bool invalidModel = false;
                var message = "";
                if (!ModelState.IsValid)
                {
                    invalidModel = true;
                    message = "Incorrect json format";
                }
                if (!account.Type.Equals(AccountType.CreditCard))
                {
                    invalidModel = true;
                    message = "Invalid account type";
                }

                if (invalidModel)
                {
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }


                var accountId = AccountsLayer.PutCreditCardAccount(account);
                if (accountId != -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, accountId);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }
        [HttpPost]
        [Route("api/accounts/checking/{accountId:int}")]
        [Authorize(Roles = "Employee,SystemManager")]
        public HttpResponseMessage updateCheckingAccount(int accountId, [FromBody]CheckingAccount account)
        {
            //
            try
            {
                var message = "";
                if (!ModelState.IsValid)
                {
                    message = "Incorrect json format";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                if (!account.Type.Equals(AccountType.Checking))
                {
                    message = "Invalid account type";
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }



                var resaccountId = AccountsLayer.PostCheckingAccount(accountId, account);
                if (resaccountId != -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, accountId);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        //TODO: put authorize attribute
        [HttpPost]
        [Route("api/accounts/savings/{accountId:int}")]
        [Authorize(Roles = "Employee,SystemManager")]
        public HttpResponseMessage updateSavingsAccount(int accountId, [FromBody]SavingsAccount account)
        {
            //
            try
            {
                bool invalidModel = false;
                var message = "";
                if (!ModelState.IsValid)
                {
                    invalidModel = true;
                    message = "Incorrect json format";
                }
                if (!account.Type.Equals(AccountType.Savings))
                {
                    invalidModel = true;
                    message = "Invalid account type";
                }

                if (invalidModel)
                {
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }


                var resaccountId = AccountsLayer.PostSavingsAccount(accountId, account);
                if (resaccountId != -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, accountId);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        //TODO: put authorize attribute
        [HttpPost]
        [Route("api/accounts/creditcard/{accountId:int}")]
        [Authorize(Roles = "Employee,SystemManager")]
        public HttpResponseMessage updateCreditCardAccount(int accountId, [FromBody]CreditCardAccount account)
        {
            try
            {
                bool invalidModel = false;
                var message = "";
                if (!ModelState.IsValid)
                {
                    invalidModel = true;
                    message = "Incorrect json format";
                }
                if (!account.Type.Equals(AccountType.CreditCard))
                {
                    invalidModel = true;
                    message = "Invalid account type";
                }

                if (invalidModel)
                {
                    HttpError err = new HttpError(message);
                    err.Message = message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }


                var resaccountId = AccountsLayer.PostCreditCardAccount(accountId, account);
                if (resaccountId != -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, accountId);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        //TODO: put authorize attribute
        [HttpDelete]
        [Route("api/accounts")]
        [Authorize(Roles = "Employee,SystemManager")]
        public HttpResponseMessage deleteAccount(int accountId)
        {
            try
            {
                var checkingAccount = AccountsLayer.GetCheckingAccountByAccountID(accountId);
                if(checkingAccount != null)
                {
                    checkingAccount.Status = AccountStatus.Closed;
                    checkingAccount.CloseDate = DateTime.Now;
                    AccountsLayer.PostCheckingAccount(accountId, checkingAccount);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                var savingsAccount = AccountsLayer.GetSavingsAccountByAccountID(accountId);
                if (savingsAccount != null)
                {
                    savingsAccount.Status = AccountStatus.Closed;
                    savingsAccount.CloseDate = DateTime.Now;
                    AccountsLayer.PostSavingsAccount(accountId, savingsAccount);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                var creditCardAccount = AccountsLayer.GetCreditCardAccountByAccountID(accountId);
                if (creditCardAccount != null)
                {
                    creditCardAccount.Status = AccountStatus.Closed;
                    creditCardAccount.CloseDate = DateTime.Now;
                    AccountsLayer.PostCreditCardAccount(accountId, creditCardAccount);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                var message = "No account exists for this accountId";
                HttpError err = new HttpError(message);
                err.Message = message;
                return Request.CreateResponse(HttpStatusCode.NotFound, err);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
