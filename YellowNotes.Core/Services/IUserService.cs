using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Services
{
    public interface IUserService
    {
        bool CreateUser(UserDto user);

        bool VerifyPassword(UserDto user);

        bool ChangePassword(UserDto user);
    }
}
