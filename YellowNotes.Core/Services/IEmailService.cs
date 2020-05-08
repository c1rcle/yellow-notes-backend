using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Email;

namespace YellowNotes.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage emailMessage, CancellationToken cancellationToken);
    }
}
