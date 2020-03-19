using System;
using System.Threading.Tasks;
using System.Web.Helpers;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;
using YellowNotes.Core.Repositories;

namespace YellowNotes.Core.Services
{
    public class UserService : IUserService
    {
        private IUserRepository repository;

        public UserService(IUserRepository repository) => this.repository = repository;

        public async Task<bool> CreateUser(UserDto user)
        {
            return await repository.CreateUser(new User
            {
                Email = user.Email,
                AccountCreationDate = DateTime.Now,
                PasswordHash = Crypto.HashPassword(user.Password)
            });
        }

        public async Task<bool> VerifyPassword(UserDto user)
        {
            return await repository.VerifyPassword(user);
        }

        public async Task<bool> ChangePassword(UserDto user)
        {
            return await repository.ChangePassword(user);
        }
    }
}
