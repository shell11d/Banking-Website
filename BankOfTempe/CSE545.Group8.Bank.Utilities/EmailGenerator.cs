using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Web;
namespace CSE545.Group8.Bank.Utilities
{
    public class EmailGenerator
    {
        private static EmailGenerator _emailGenerator;
        private static List<Credential> credentials;

        private EmailGenerator()
        {
            credentials = new List<Credential>()
            {
                new Credential("first.last.group8.1@gmail.com", "myPassword1"),
                new Credential("first.last.group8.2@gmail.com", "myPassword2"),
                new Credential("first.last.group8.3@gmail.com", "myPassword3"),
                new Credential("first.last.group8.4@gmail.com", "myPassword4"),
                new Credential("first.last.group8.5@gmail.com", "myPassword5"),
                new Credential("first.last.group8.6@gmail.com", "myPassword6"),
                new Credential("first.last.group8.7@gmail.com", "myPassword7"),
                new Credential("first.last.group8.8@gmail.com", "myPassword8"),
                new Credential("first.last.group8.9@gmail.com", "myPassword9"),
                new Credential("first.last.group8.10@gmail.com", "myPassword10")
            };
        }
        public static EmailGenerator Instance
        {
            get
            {
                if (_emailGenerator == null)
                {
                    _emailGenerator = new EmailGenerator();
                }
                return _emailGenerator;
            }
        }
        public void sendEmail(string To, string emailbody, string subject, string attachmentPath, bool IsBodyHtml)
        {
            try
            {
                var rand = new Random();
                var index = rand.Next(0, credentials.Count() - 1);
                var credential = credentials[index];
                var smtpclient = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(credential.UserName, credential.Password)
                };

                var mailmsg = new MailMessage(credential.UserName, To, subject, emailbody);
                mailmsg.IsBodyHtml = IsBodyHtml;
                mailmsg.BodyEncoding = UTF8Encoding.UTF8;
                if (!string.IsNullOrWhiteSpace(attachmentPath))
                {
                    var attachment = new Attachment(attachmentPath);
                    mailmsg.Attachments.Add(attachment);
                }
                mailmsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                smtpclient.Send(mailmsg);

            }

            catch (Exception ex)
            {
                throw new Exception("Error Sending Email. Please try again later", ex);
            }
        }

        static void Main()
        {
            string to = "ddnyannmothe@gmail.com";
            string emailbody = "Hello World";
            string subject = "Not Important";
            //sendEmail(to, emailbody, subject, null);
        }
    }

    public struct Credential
    {
        public string UserName;
        public string Password;

        public Credential(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }

}
