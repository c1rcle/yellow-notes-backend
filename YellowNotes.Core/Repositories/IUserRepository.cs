using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user, CancellationToken cancellationToken);

        Task<bool> VerifyPassword(UserDto user, CancellationToken cancellationToken);

        Task<bool> ChangePassword(UserDto user, CancellationToken cancellationToken);
    }
}
