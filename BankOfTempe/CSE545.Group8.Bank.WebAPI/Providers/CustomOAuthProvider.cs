using CSE545.Group8.Bank.WebAPI.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CSE545.Group8.Bank.WebAPI.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                var existingUser = await userManager.FindByNameAsync(context.UserName);
                if (existingUser == null)
                {
                    context.SetError("invalid_grant", string.Format("The user name or password is incorrect."));
                    return;
                }
                var user = await userManager.FindAsync(context.UserName, context.Password);

                if (await userManager.IsLockedOutAsync(existingUser.Id))
                {
                    context.SetError(string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));
                    return;
                }

                if (user == null)
                {
                    var maxAllowedFailedAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"].ToString());

                    await userManager.AccessFailedAsync(existingUser.Id);
                    int accessFailedCount = await userManager.GetAccessFailedCountAsync(existingUser.Id);
                    if (accessFailedCount == 0)
                    {
                        accessFailedCount = maxAllowedFailedAttempts;
                    }
                    context.SetError("invalid_grant", string.Format("The user name or password is incorrect. {0}/{1} failed attempts", accessFailedCount, maxAllowedFailedAttempts));
                    return;
                }
                if (await userManager.IsLockedOutAsync(user.Id))
                {
                    context.SetError(string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));
                    return;
                }
                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "User did not confirm email.");
                    return;
                }


                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
                oAuthIdentity.AddClaim(new Claim("PSK", user.PSK));

                var ticket = new AuthenticationTicket(oAuthIdentity, null);

                await userManager.ResetAccessFailedCountAsync(user.Id);

                context.Validated(ticket);
            }
            catch(Exception e)
            {
                context.SetError("Internal Server Error");
                return;
            }

        }
    }
}