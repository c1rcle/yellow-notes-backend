﻿using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace YellowNotes.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetEmailFromClaims(this HttpContext httpContext)
        {
            var emailClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return null;
            }
            return emailClaim.Value;
        }
    }
}