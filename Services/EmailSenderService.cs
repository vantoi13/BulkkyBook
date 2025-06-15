using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using BulkkyBook.Utils.ConfigOptions.Email;
using MailKit.Security;

namespace BulkkyBook.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSenderService(IOptions<EmailSettings> emailOptions)
        {
            _emailSettings = emailOptions.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));

            var emailList = email.Split(",");
            foreach (var e in emailList)
            {
                emailMessage.To.Add(new MailboxAddress("", e));
            }
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using (var client = new SmtpClient())
            {
                // await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);

                await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}