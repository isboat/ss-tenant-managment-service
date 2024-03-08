using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Tenancy.Management.Models;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings; // Host = smtp.gmail.com, Port = 587

        public EmailSender(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient();
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("OnScreenSync Management Service", _settings.FromAddress));
                mimeMessage.To.Add(MailboxAddress.Parse(email));

                mimeMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = message,
                    TextBody = "For more info visit our site for more details"
                };

                mimeMessage.Body = bodyBuilder.ToMessageBody();
                smtpClient.Connect(_settings.Host, _settings.Port, true);
                smtpClient.Authenticate(_settings.FromAddress, _settings.Passkey);
                await smtpClient.SendAsync(mimeMessage);
            }
            catch (Exception ex)
            {
                // log
                throw;
            }
            finally
            {
                smtpClient.Disconnect(true);
                smtpClient.Dispose();
            }
        }
    }
}
