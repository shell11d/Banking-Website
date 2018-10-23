using CSE545.Group8.Bank.DataAccessLayer;
using CSE545.Group8.Bank.Entities;
using System;
using CSE545.Group8.Bank.Validators;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;

namespace CSE545.Group8.Bank.WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route("api/users/userEmail")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        [ResponseType(typeof(User))]
        public HttpResponseMessage GetUserByEmail(string userEmail)
        {
            try
            {
                var sanitizer = new Sanitizer();
                if (!sanitizer.IsValidEmail(userEmail))
                {
                    var message = string.Format("Email address format invalid", userEmail);
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                var user = UserLayer.GetUserByEmailId(userEmail);
                if (user != null)
                {
                    return Request.CreateResponse<User>(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        [Route("api/users/merchants")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        [ResponseType(typeof(List<Merchant>))]
        public HttpResponseMessage GetAllMerchants()
        {
            try
            {
                List<Merchant> merchantList = UserLayer.GetAllMerchantsWithAccounts();
                if (merchantList != null)
                {
                    return Request.CreateResponse<List<Merchant>>(HttpStatusCode.OK, merchantList);
                }
                else
                {
                    var message = string.Format("No merchants found");
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
        }


        //TODO: Get all users who are merchants
        [HttpGet]
        [Route("api/users/phoneNumber")]
        [ResponseType(typeof(User))]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage GetUserByPhoneNumber(string phoneNumber)
        {
            try
            {


                var sanitizer = new Sanitizer();
                if (!sanitizer.validatePhoneNumber(phoneNumber))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(string.Format("Invalid Phone Number {0} ", phoneNumber)));
                }

                var user = UserLayer.GetUserByPhoneNumber(phoneNumber);
                if (user != null)
                    return Request.CreateResponse<User>(HttpStatusCode.OK, user);
                else
                {
                    var message = string.Format("User doesn't exist");
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
        [Route("api/users")]
        //[Authorize(Roles = "Administrator,Employee,SystemManager")]
        public HttpResponseMessage CreateUser([FromBody]User user)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new HttpError("Incorrect json format"));
                }
                user.FullName = user.FirstName + user.LastName;

                if (user.Type == UserType.Individual || user.Type == UserType.Merchant)
                {
                    if (UserLayer.PutCustomer(user))
                        return Request.CreateResponse(HttpStatusCode.OK);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                else if (user.Type == UserType.SystemManager || user.Type == UserType.Employee || user.Type == UserType.Administrator)
                {
                    if (UserLayer.PutEmployee(user))
                        return Request.CreateResponse(HttpStatusCode.OK);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("User Type Not Defined"));
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        [Route("api/users/userEmail")]
        public HttpResponseMessage ModifyUserDetailsbyEmail(string userEmail, [FromBody]User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new HttpError("Incorrect json format"));
                }

                var sanitizer = new Sanitizer();
                if (!sanitizer.IsValidEmail(userEmail))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new HttpError(string.Format("Email address format invalid {0}", userEmail)));
                }
                var existingUser = UserLayer.GetUserByEmailId(userEmail);

                if (existingUser != null)
                {
                    if (existingUser.Type == UserType.Individual || existingUser.Type == UserType.Merchant)
                    {
                        if (UserLayer.PostCustomer(existingUser.UserId, user))
                            return Request.CreateResponse(HttpStatusCode.OK);
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);

                    }
                    else if (existingUser.Type == UserType.Employee || existingUser.Type == UserType.SystemManager || existingUser.Type == UserType.Administrator)
                    {
                        if (UserLayer.PostEmployee(existingUser.UserId, user))
                            return Request.CreateResponse(HttpStatusCode.OK);
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("User Type Not Defined"));
                    }
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

        }

    }
}

