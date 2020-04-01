using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Utility
{
    public static class TokenUtility
    {
        public static string Authorize(string userEmail, IHeaderDictionary httpHeaders)
        {
            string error = null;
            string token = ParseFromHeaders(httpHeaders);
            if (token == null)
            {
                error = "No token";
            }

            bool valid = Validate(token, userEmail);
            if (!valid)
            {
                error = "Bad token";
            }
            return error;
        }

        private static string ParseFromHeaders(IHeaderDictionary httpHeaders)
        {
            var success = httpHeaders.TryGetValue("Authorization", out var authorizationString);
            if (!success)
                return null;

            var token = authorizationString.ToString().Split(' ')[1];
            return token;
        }

        private static bool Validate(string token, string userEmail)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var decodedEmail = securityToken.Payload["email"] as string;

            var isUserAuthorized = decodedEmail == userEmail;
            var tokenExpired = securityToken.ValidTo < DateTime.UtcNow;

            return isUserAuthorized && !tokenExpired;
        }
    }
}
