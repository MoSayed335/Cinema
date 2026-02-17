using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Cinema.Utility
{
    public class EmailSend : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mo.sayed2037@gmail.com", "fend clvh jvhw huqx")
            };
            return client.SendMailAsync(
                new MailMessage(from: "mo.sayed2037@gmail.com",
                to: email,
                subject,
                htmlMessage)
                {
                    IsBodyHtml = true
                }
                );
        }
    }
}
