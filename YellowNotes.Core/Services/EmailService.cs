using System.Threading.Tasks;
using YellowNotes.Core.Email;
using MimeKit;
using MailKit.Net.Smtp;

namespace YellowNotes.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public async Task SendEmail(EmailMessage emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailMessage.fromEmailAddress));
            email.To.Add(new MailboxAddress(emailMessage.toEmailAddress));
            email.Subject = emailMessage.subject;

            var body = new BodyBuilder
            {
                HtmlBody = emailMessage.content
            };

            using (var client = new SmtpClient())
	        {
                client.ServerCertificateValidationCallback = (sender, certificate, certChainType, errors) => true;
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false).ConfigureAwait(false);
                await client.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword).ConfigureAwait(false);

		        await client.SendAsync(email).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
