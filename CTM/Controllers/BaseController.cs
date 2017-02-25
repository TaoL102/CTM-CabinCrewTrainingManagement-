using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTM.Codes.Attributes;

namespace CTM.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Localization
        /// Reference:http://afana.me/post/aspnet-mvc-internationalization.aspx
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                                    null;

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

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