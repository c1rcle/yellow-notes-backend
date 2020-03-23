using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoHelper;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;
using YellowNotes.Core.Repositories;

namespace YellowNotes.Core.Services
{
    public class UserService : IUserService
    {
        private IUserRepository repository;

        public UserService(IUserRepository repository) => this.repository = repository;

        public async Task<bool> CreateUser(UserDto user, CancellationToken cancellationToken)
        {
            return await repository.CreateUser(new User
            {
                Email = user.Email,
                RegistrationDate = DateTime.Now,
                PasswordHash = Crypto.HashPassword(user.Password)
            }, cancellationToken);
        }

        public async Task<bool> VerifyPassword(UserDto user, CancellationToken cancellationToken)
        {
            return await repository.VerifyPassword(user, cancellationToken);
        }

        public async Task<bool> ChangePassword(UserDto user, CancellationToken cancellationToken)
        {
            return await repository.ChangePassword(user, cancellationToken);
        }
    }
}
