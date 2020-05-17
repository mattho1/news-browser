using Backend.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfig _emailConfig;

        public EmailSender(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var email = CreateEmailMessage(message);
            await Send(email);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailConfig.From));
            email.To.Add(new MailboxAddress(message.Receiver));
            email.Subject = message.Title;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return email;
        }

        private async Task Send(MimeMessage message)
        {
            using (var clientSmtp = new SmtpClient())
            {
                try
                {
                    clientSmtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    clientSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    clientSmtp.Timeout = 30000;
                    clientSmtp.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    await clientSmtp.SendAsync(message);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    clientSmtp.Disconnect(true);
                    clientSmtp.Dispose();
                }
            }
        }
    }
}
