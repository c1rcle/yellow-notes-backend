using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CryptoHelper;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext context;

        public UserRepository(DatabaseContext context) => this.context = context;

        public async Task<bool> CreateUser(User user, CancellationToken cancellationToken)
        {
            context.Users.Add(user);
            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> VerifyPassword(UserDto user, CancellationToken cancellationToken)
        {
            var record = await context.Users
                .SingleOrDefaultAsync(x => x.Email == user.Email, cancellationToken);

            return record != null &&
                Crypto.VerifyHashedPassword(record.PasswordHash, user.Password);
        }

        public async Task<bool> ChangePassword(UserDto user, CancellationToken cancellationToken)
        {
            var record = await context.Users
                .SingleOrDefaultAsync(x => x.Email == user.Email, cancellationToken);

            if (!Crypto.VerifyHashedPassword(record.PasswordHash, user.Password))
            {
                return false;
            }

            record.PasswordHash = Crypto.HashPassword(user.NewPassword);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
