using System.Threading.Tasks;
using System.Threading;
using YellowNotes.Core.Email;

namespace YellowNotes.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage emailMessage, CancellationToken cancellationToken);
    }
}
