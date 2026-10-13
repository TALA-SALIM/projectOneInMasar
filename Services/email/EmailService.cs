using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace homework.Services.email
{
    public class EmailService : IEmailService
    {
        public async Task Send(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(
                    Environment.GetEnvironmentVariable("EMAIL_USERNAME"),
                    "Product App"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            message.To.Add(new MailAddress(to));

            try
            {
                using var emailClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                        Environment.GetEnvironmentVariable("EMAIL_USERNAME"),
                        Environment.GetEnvironmentVariable("EMAIL_APP_PASSWORD"))
                };

                await emailClient.SendMailAsync(message);
            }
            catch (Exception)
            {
            }
        }
    }
}