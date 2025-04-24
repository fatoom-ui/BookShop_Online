//using Microsoft.AspNetCore.Identity.UI.Services;
//using System.Net;
//using System.Net.Mail;
//using System.Reflection;

//namespace BookShop.Services
//{
//    public class EmailSender : IEmailSender
//    {
//        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//        {
//            var fromemail = "nour@gmail.com";
//            var frompassword = @"";
//            MailMessage message = new();
//            message.From = new MailAddress(fromemail);
//            message.To.Add(new MailAddress(email));
//            message.Subject = subject;
//            message.Body = htmlMessage;
//            message.IsBodyHtml = true;
//            var smptClient = new SmtpClient("smpt@gmail.com")
//            {
//                Port = 587,
//                Credentials = new NetworkCredential(fromemail, frompassword),
//                EnableSsl = true

//            };
//            smptClient.Send(message);
//        }
//    }
//}
