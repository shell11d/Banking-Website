using CSE545.Group8.Bank.WebAPI.Services;
using CSE545.Group8.Bank.WebAPI.Validators;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CSE545.Group8.Bank.WebAPI.Infrastructure
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));
            appUserManager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            appUserManager.UserLockoutEnabledByDefault = Convert.ToBoolean(ConfigurationManager.AppSettings["UserLockoutEnabledByDefault"].ToString());
            appUserManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(Double.Parse(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));
            appUserManager.MaxFailedAccessAttemptsBeforeLockout = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"].ToString());
        
            //Custom Logic to be modified from here 

            //appUserManager.UserValidator = new CustomUserValidator(appUserManager)
            //{
            //    AllowOnlyAlphanumericUserNames = true,
            //    RequireUniqueEmail = true
            //};

            //appUserManager.PasswordValidator = new CustomPasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = false,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};

            return appUserManager;
        }
    }
}