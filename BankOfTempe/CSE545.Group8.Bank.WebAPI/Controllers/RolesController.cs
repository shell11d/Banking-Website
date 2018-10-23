using CSE545.Group8.Bank.WebAPI.Infrastructure;
using CSE545.Group8.Bank.WebAPI.Models;
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
using static CSE545.Group8.Bank.WebAPI.Models.RoleBindingModels;

namespace CSE545.Group8.Bank.WebAPI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [RoutePrefix("api/roles")]
    public class RolesController : ApiController
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
        [Route("{id:guid}", Name = "GetRoleById")]
        public IHttpActionResult GetRole(string Id)
        {
            try
            {
                var role = this.AppRoleManager.FindById(Id);

                if (role != null)
                {
                    return Ok(TheModelFactory.Create(role));
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }

        }

        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            try
            {
                var roles = this.AppRoleManager.Roles;

                return Ok(roles);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Route("create")]
        public IHttpActionResult Create(CreateRoleBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var role = new IdentityRole { Name = model.Name };

                var result = this.AppRoleManager.Create(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

                return Created(locationHeader, TheModelFactory.Create(role));
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Route("{id:guid}")]
        public IHttpActionResult DeleteRole(string Id)
        {
            try
            {
                var role = this.AppRoleManager.FindById(Id);

                if (role != null)
                {
                    IdentityResult result = this.AppRoleManager.Delete(role);

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

        [Route("ManageUsersInRole")]
        public IHttpActionResult ManageUsersInRole(UsersInRoleModel model)
        {
            try
            {
                var role = this.AppRoleManager.FindById(model.Id);

                if (role == null)
                {
                    ModelState.AddModelError("", "Role does not exist");
                    return BadRequest(ModelState);
                }

                foreach (string user in model.EnrolledUsers)
                {
                    var appUser = this.AppUserManager.FindById(user);

                    if (appUser == null)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                        continue;
                    }
                    if (!this.AppUserManager.IsInRole(user, role.Name))
                    {
                        IdentityResult result = this.AppUserManager.AddToRole(user, role.Name);

                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                        }
                    }
                }

                foreach (string user in model.RemovedUsers)
                {
                    var appUser = this.AppUserManager.FindById(user);

                    if (appUser == null)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                        continue;
                    }

                    IdentityResult result = this.AppUserManager.RemoveFromRole(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                    }
                }
                if (!ModelState.IsValid)
                {
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
