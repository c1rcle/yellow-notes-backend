using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user);

        Task<bool> VerifyPassword(UserDto user);

        Task<bool> ChangePassword(UserDto user);
    }
}
