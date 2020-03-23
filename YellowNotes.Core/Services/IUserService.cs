using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserDto user, CancellationToken cancellationToken);

        Task<bool> VerifyPassword(UserDto user, CancellationToken cancellationToken);

        Task<bool> ChangePassword(UserDto user, CancellationToken cancellationToken);

        string GenerateJWT(UserDto user);
      
        bool ValidateToken(string token, UserDto user);
    }
}
