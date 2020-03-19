using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public interface IUserRepository
    {
        bool CreateUser(User user);

        bool VerifyPassword(User user);

        bool ChangePassword(User user);
    }
}
