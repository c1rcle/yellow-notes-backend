using Microsoft.AspNetCore.Http;

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
    }
}
