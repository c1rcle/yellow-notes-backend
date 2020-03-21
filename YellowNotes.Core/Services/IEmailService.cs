using System.Threading.Tasks;
using System.Threading;

namespace YellowNotes.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage emailMessage, CancellationToken cancellationToken);
    }
}
