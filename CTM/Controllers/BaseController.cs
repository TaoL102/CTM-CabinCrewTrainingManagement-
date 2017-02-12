using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CTM.Controllers
{
    public class BaseController : Controller
    {

        public async Task SendEmail(string emailTo, string emailSubject, string emailBody)
        {
            emailTo = HttpUtility.UrlDecode(emailTo);
            if (string.IsNullOrEmpty(emailTo))
            {
                return;
            }
            emailSubject = HttpUtility.UrlDecode(emailSubject)??string.Empty;
            emailBody = HttpUtility.UrlDecode(emailBody) ?? string.Empty;

            var message = new MailMessage();
            message.To.Add(new MailAddress(emailTo)); // replace with valid value 
            message.From = new MailAddress("liu.tao@outlook.com"); // replace with valid value
            message.Subject = "Error Report (" + emailSubject + ")";
            message.Body = string.Format(emailBody);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName =Environment.GetEnvironmentVariable("EmailUserName") ,
                    Password = Environment.GetEnvironmentVariable("EmailPassword")
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
    }
}