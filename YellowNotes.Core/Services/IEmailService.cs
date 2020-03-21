using System.Threading.Tasks;

namespace YellowNotes.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage emailMessage);
    }
}
