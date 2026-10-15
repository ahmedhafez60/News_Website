
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace WebApplication1.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage)
        {
            var settings = _configuration.GetSection("EmailSettings");

            var senderEmail = settings["Email"];
            var appPassword = settings["AppPassword"];
            var host = settings["Host"] ?? "smtp.gmail.com";
            var port = int.Parse(settings["Port"] ?? "587");

            if (string.IsNullOrWhiteSpace(senderEmail) ||
                string.IsNullOrWhiteSpace(appPassword))
            {
                throw new InvalidOperationException(
                    "Email settings are missing.");
            }

            using var message = new MailMessage();
            message.From = new MailAddress(senderEmail);
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(host, port);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(
                senderEmail, appPassword);

            await client.SendMailAsync(message);
        }
    }
}
