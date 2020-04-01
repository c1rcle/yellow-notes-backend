using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Utility
{
    public static class TokenParser
    {
        public static string FromHeaders(IHeaderDictionary httpHeaders)
        {
            var success = httpHeaders.TryGetValue("Authorization", out var authorizationString);
            if (!success)
                return null;

            var token = authorizationString.ToString().Split(' ')[1];
            return token;
        }

        public static bool ValidateToken(string token, UserDto user)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var decodedEmail = securityToken.Payload["email"] as string;

            var isUserAuthorized = decodedEmail == user.Email;
            var tokenExpired = securityToken.ValidTo < DateTime.UtcNow;

            return isUserAuthorized && !tokenExpired;
        }
    }
}
