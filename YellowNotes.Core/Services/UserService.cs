using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public UserService(IUserRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }

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

        public string GenerateJWT(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token, UserDto user)
        {
            JwtSecurityToken securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            string decodedEmail = securityToken.Payload["email"] as string;

            bool isUserAuthorized = decodedEmail == user.Email;
            bool tokenExpired = securityToken.ValidTo < DateTime.UtcNow;

            return isUserAuthorized && !tokenExpired;
        }

    }
}
