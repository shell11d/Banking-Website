using CSE545.Group8.Bank.WebAPI.Infrastructure;
using CSE545.Group8.Bank.WebAPI.Models;
using CSE545.Group8.Bank.WebAPI.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static CSE545.Group8.Bank.WebAPI.Models.UserBindingModels;

namespace CSE545.Group8.Bank.WebAPI.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        [Authorize(Roles = "Administrator,SystemManager")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Authorize(Roles = "Administrator,SystemManager")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public IHttpActionResult GetUser(string Id)
        {
            try
            {
                var user = this.AppUserManager.FindById(Id);

                if (user != null)
                {
                    return Ok(this.TheModelFactory.Create(user));
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Authorize(Roles = "Administrator,SystemManager")]
        [Route("user/{username}")]
        public IHttpActionResult GetUserByName(string username)
        {
            try
            {
                var user = this.AppUserManager.FindByName(username);

                if (user != null)
                {
                    return Ok(this.TheModelFactory.Create(user));
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Authorize(Roles = "Administrator,SystemManager,Employee")]
        [Route("create")]
        public IHttpActionResult CreateUser(CreateUserBindingModel createUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser()
                {
                    UserName = createUserModel.Username,
                    Email = createUserModel.Email,
                    FirstName = createUserModel.FirstName,
                    LastName = createUserModel.LastName,
                    TwoFactorEnabled = true,
                    EmailConfirmed = false,
                    PSK = TimeSensitivePassCode.GeneratePresharedKey(10)
                };

                IdentityResult addUserResult = this.AppUserManager.Create(user, createUserModel.Password);

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }

                var rolesToAdd = new List<string>();
                foreach (var userType in createUserModel.UserTypes)
                {
                    if (userType != Entities.UserType.Invalid)
                    {
                        var role = userType.ToString();
                        rolesToAdd.Add(role);
                        if (!this.AppRoleManager.RoleExists(role))
                        {
                            this.AppRoleManager.Create(new IdentityRole { Name = role });
                        }
                    }
                }

                var newlyCreatedUser = this.AppUserManager.FindByName(createUserModel.Username);

                this.AppUserManager.AddToRoles(newlyCreatedUser.Id, rolesToAdd.ToArray());

                string code = this.AppUserManager.GenerateEmailConfirmationToken(user.Id);

                var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

                this.AppUserManager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

                return Created(locationHeader, TheModelFactory.Create(user));

            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public IHttpActionResult ConfirmEmail(string userId = "", string code = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
                {
                    ModelState.AddModelError("", "User Id and Code are required");
                    return BadRequest(ModelState);
                }

                IdentityResult result = this.AppUserManager.ConfirmEmail(userId, code);

                if (result.Succeeded)
                {
                    var approvedUser = this.AppUserManager.FindById(userId);
                    try
                    {
                        this.AppUserManager.SendEmail(approvedUser.Id, "Your Google Authenticator Key for Bank of Tempe", "our Email has been Verified. Please Setup Google Authenticator using this Key: " + approvedUser.PSK);
                    }
                    catch (Exception e)
                    { }
                    return Ok();
                }
                else
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("ForgotPassword", Name = "ForgotPasswordRoute")]
        public IHttpActionResult ForgotPassword(ForgotPasswordBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = AppUserManager.FindByEmail(model.Email);
                    if (user == null || !(AppUserManager.IsEmailConfirmed(user.Id)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return Ok();
                    }

                    var passwordResetToken = this.AppUserManager.GeneratePasswordResetToken(user.Id);
                    var newPassword = TimeSensitivePassCode.GeneratePresharedKey(8);
                    var random = new Random();
                    var smallChars = "abcdefghijklmnopqrstuvwxyz";
                    var specialChars = "!@#$%^&*()";
                    var additionalCharacters = smallChars[random.Next(smallChars.Length)].ToString() + specialChars[random.Next(specialChars.Length)].ToString();
                    newPassword += additionalCharacters;
                    var result = this.AppUserManager.ResetPassword(user.Id, passwordResetToken, newPassword);

                    if (result.Succeeded)
                        AppUserManager.SendEmail(user.Id, "Reset Password", "<div>" + newPassword + "<div>");
                    return Ok();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(ChangePasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                IdentityResult result = this.AppUserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Authorize(Roles = "Administrator")]
        [Route("user/{id:guid}")]
        public IHttpActionResult DeleteUser(string id)
        {
            try
            {
                var appUser = this.AppUserManager.FindById(id);

                if (appUser != null)
                {
                    IdentityResult result = this.AppUserManager.Delete(appUser);

                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Authorize(Roles = "Administrator")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public IHttpActionResult AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            try
            {
                var appUser = this.AppUserManager.FindById(id);

                if (appUser == null)
                {
                    return NotFound();
                }

                var currentRoles = this.AppUserManager.GetRoles(appUser.Id);

                var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

                if (rolesNotExists.Count() > 0)
                {

                    ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                    return BadRequest(ModelState);
                }

                IdentityResult removeResult = this.AppUserManager.RemoveFromRoles(appUser.Id, currentRoles.ToArray());

                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to remove user roles");
                    return BadRequest(ModelState);
                }

                IdentityResult addResult = this.AppUserManager.AddToRoles(appUser.Id, rolesToAssign);

                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to add user roles");
                    return BadRequest(ModelState);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }
    }
}
