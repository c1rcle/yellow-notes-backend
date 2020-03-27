using System.Threading.Tasks;
using System.Threading;
using YellowNotes.Core.Email;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit.Text;

namespace YellowNotes.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration emailConfiguration;

        public EmailService(IOptionsMonitor<EmailConfiguration> emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration.CurrentValue;
        }

        public async Task SendEmail(EmailMessage emailMessage, CancellationToken cancellationToken)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Yellow Notes", emailMessage.FromEmailAddress));
            email.To.Add(new MailboxAddress(emailMessage.ToEmailAddress));
            email.Subject = emailMessage.Subject;

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback =
                    (sender, certificate, certChainType, errors) => true;
                await client.ConnectAsync(emailConfiguration.SmtpServer,
                    emailConfiguration.SmtpPort, true, cancellationToken);
                await client.AuthenticateAsync(emailConfiguration.SmtpUsername,
                    emailConfiguration.SmtpPassword, cancellationToken);

                await client.SendAsync(email, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}
