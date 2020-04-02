using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Utility
{
    public static class TokenUtility
    {
        public static string Validate(string userEmail, IHeaderDictionary httpHeaders)
        {
            string error = null;

            string token = ParseFromHeaders(httpHeaders);
            if (token == null)
            {
                return "No token";
            }

            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var decodedEmail = securityToken.Payload["email"] as string;

            var isUserAuthorized = decodedEmail == userEmail;
            var tokenExpired = securityToken.ValidTo < DateTime.UtcNow;

            var validToken = isUserAuthorized && !tokenExpired;
            if (!validToken)
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
    }
}
