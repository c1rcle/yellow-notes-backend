using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace YellowNotes.Core.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string fromEmailAddress, string toEmailAddress, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromEmailAddress));
            email.To.Add(new MailboxAddress(toEmailAddress));
            email.Subject = subject;

            var body = new BodyBuilder
            {
                HtmlBody = message
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, certChainType, errors) => true;
                await client.ConnectAsync("smpt.host", 000, false).ConfigureAwait(false);
                await client.AuthenticateAsync("username", "password").ConfigureAwait(false);

                await client.SendAsync(email).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
