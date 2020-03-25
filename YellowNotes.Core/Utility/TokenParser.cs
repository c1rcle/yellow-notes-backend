using Microsoft.AspNetCore.Http;

namespace YellowNotes.Core.Utility
{
    public static class TokenParser
    {
        public static string FromHeaders(IHeaderDictionary httpHeaders)
        {
            bool success = httpHeaders.TryGetValue("Authorization", out var authorizationString);
            if (!success)
                return null;

            string token = authorizationString.ToString().Split(' ')[1];
            return token;
        }
    }
}
