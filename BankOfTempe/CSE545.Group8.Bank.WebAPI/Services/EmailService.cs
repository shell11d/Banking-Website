using Microsoft.AspNet.Identity;
using System;
using SendGrid;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using CSE545.Group8.Bank.Utilities;

namespace CSE545.Group8.Bank.WebAPI.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            //await configSendGridasync(message);
            var emailSender = EmailGenerator.Instance;

            emailSender.sendEmail(message.Destination,message.Body,message.Subject,null,true);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        //private async Task configSendGridasync(IdentityMessage message)
        //{
        //    var myMessage = new SendGridMessage();
            
        //    myMessage.AddTo(message.Destination);
        //    myMessage.From = new System.Net.Mail.MailAddress("taiseer@bitoftech.net", "Taiseer Joudeh");
        //    myMessage.Subject = message.Subject;
        //    myMessage.Text = message.Body;
        //    myMessage.Html = message.Body;

        //    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
        //                                            ConfigurationManager.AppSettings["emailService:Password"]);

        //    // Create a Web transport for sending email.
        //    var transportWeb = new Web(credentials);

        //    // Send the email.
        //    if (transportWeb != null)
        //    {
        //        await transportWeb.DeliverAsync(myMessage);
        //    }
        //    else
        //    {
        //        //Trace.TraceError("Failed to create Web transport.");
        //        await Task.FromResult(0);
        //    }
        //}
    }
}