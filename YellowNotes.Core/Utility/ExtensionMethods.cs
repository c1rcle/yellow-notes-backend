using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace YellowNotes.Core.Utility
{
    public static class ExtensionMethods
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
