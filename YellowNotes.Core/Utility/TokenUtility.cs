using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Utility
{
    public static class TokenUtility
    {
        private static string ParseFromHeaders(IHeaderDictionary httpHeaders)
        {
            var success = httpHeaders.TryGetValue("Authorization", out var authorizationString);
            if (!success)
                return null;

            var token = authorizationString.ToString().Split(' ')[1];
            return token;
        }

        private static bool Validate(string token, UserDto user)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var decodedEmail = securityToken.Payload["email"] as string;

            var isUserAuthorized = decodedEmail == user.Email;
            var tokenExpired = securityToken.ValidTo < DateTime.UtcNow;

            return isUserAuthorized && !tokenExpired;
        }

        public static string Authorize(UserDto userDto, IHeaderDictionary httpHeaders)
        {
            string error = null;
            string token = ParseFromHeaders(httpHeaders);
            if (token == null)
            {
                error = "No token";
            }

            bool valid = Validate(token, userDto);
            if (!valid)
            {
                error = "Bad token";
            }
            return error;
        }
    }
}
