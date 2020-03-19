using System.Threading.Tasks;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserDto user);

        Task<bool> VerifyPassword(UserDto user);

        Task<bool> ChangePassword(UserDto user);
    }
}
