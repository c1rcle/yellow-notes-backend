using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;
using YellowNotes.Core.Repositories;

namespace YellowNotes.Core.Services
{
    public class UserService : IUserService
    {
        private IUserRepository repository;

        private IConfiguration configuration;

        private string jwtSecret;

        public UserService(IUserRepository repository, IConfiguration configuration,
            string jwtSecret)
        {
            this.repository = repository;
            this.configuration = configuration;
            this.jwtSecret = jwtSecret;
        }

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

        public string GenerateJwt(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(configuration.GetValue<int>("TokenExpiryHours")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
