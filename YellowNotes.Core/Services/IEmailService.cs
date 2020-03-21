using System.Threading.Tasks;

namespace YellowNotes.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(
                string fromEmailAddress,
                string toEmailAddress,
                string subject,
                string message
            );
    }
}
