using System.Threading.Tasks;
using CryptoHelper;
using Microsoft.EntityFrameworkCore;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DatabaseContext context;

        public UserRepository(DatabaseContext context) => this.context = context;

        public async Task<bool> CreateUser(User user)
        {
            var emailExists = await context.Users
                .AnyAsync(x => x.Email == user.Email);

            if (emailExists) return false;
            context.Users.Add(user);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> VerifyPassword(UserDto user)
        {
            var record = await context.Users
                .SingleOrDefaultAsync(x => x.Email == user.Email);

            return record != null && 
                Crypto.VerifyHashedPassword(record.PasswordHash, user.Password);
        }

        public async Task<bool> ChangePassword(UserDto user)
        {
            var record = await context.Users
                .SingleOrDefaultAsync(x => x.Email == user.Email);

            if (!Crypto.VerifyHashedPassword(record.PasswordHash, user.Password)) return false;

            record.PasswordHash = Crypto.HashPassword(user.NewPassword);
            context.Users.Update(record);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
