using MetalcutWeb.Service.Interface;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MetalcutWeb.Service.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly string fromEmail;
        private readonly string fromPass;

        public EmailSender(IConfiguration config)
        {
            _config = config;
            fromEmail = _config["AppEmail"];
            fromPass = _config["AppPassword"];
        }
        public void SendEmail(string toEmail, string subject, string body, bool isBodyHtml)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromEmail);
            mailMessage.To.Add(new MailAddress(toEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = isBodyHtml;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPass),
                EnableSsl = true,
            };

            smtpClient.Send(mailMessage);
        }
    }
}
