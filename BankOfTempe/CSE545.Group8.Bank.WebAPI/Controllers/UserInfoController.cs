using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CSE545.Group8.Bank.Entities;
using System.Web.Http.Description;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using CSE545.Group8.Bank.DataAccessLayer;
using static CSE545.Group8.Bank.WebAPI.Models.SecurityQuestionsBindingModels;

namespace CSE545.Group8.Bank.WebAPI.Controllers
{
    public class UserInfoController : ApiController
    {
        public List<UserInfo> UserInfo = new List<UserInfo>();

        [HttpPut]
        [Route("api/UserInfo")]
        [Authorize(Roles = "Administrator,Individual,Merchant,Employee,SystemManager")]
        public HttpResponseMessage CreateUserInfo([FromBody]UserInfo userInfo)
        {
            try
            {
                var result = UserInfoLayer.PutUserInfo(userInfo);
                if (result)
                    return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/userInfo/{userEmail}")]
        [ResponseType(typeof(Entities.UserInfo))]
        public HttpResponseMessage GetUserQuestions(string userEmail)
        {
            try
            {
                var userQuestions = UserInfoLayer.GetUserQuestionsByEmail(userEmail);
                return Request.CreateResponse<UserInfo>(HttpStatusCode.OK, userQuestions);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/userInfo/verify")]
        [ResponseType(typeof(Entities.UserInfo))]
        public HttpResponseMessage VerifySecurityInfoByEmail([FromBody]VerifySecurityInfoModel userInfo)
        {
            try
            {
                var dbSecurityInformation = UserInfoLayer.GetUserInfoByEmail(userInfo.Email);
                var isValid = false;

                if (dbSecurityInformation != null)
                {
                    if (string.Equals(userInfo.Answer1, dbSecurityInformation.SecurityAnswer1) && string.Equals(userInfo.Answer2, dbSecurityInformation.SecurityAnswer2) && string.Equals(userInfo.Answer3, dbSecurityInformation.SecurityAnswer3))
                    {
                        isValid = true;
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, isValid);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/userInfo")]
        public HttpResponseMessage UpdateUserInfo([FromBody]UserInfo userInfo)
        {
            try
            {
                var result = UserInfoLayer.PutUserInfo(userInfo);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
