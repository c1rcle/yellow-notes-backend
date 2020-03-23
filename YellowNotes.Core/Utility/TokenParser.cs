using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace YellowNotes.Core.Utility
{
    public static class TokenParser
    {
        public static string FromHeaders(IHeaderDictionary httpHeaders)
        {
            if (!httpHeaders.ContainsKey("Authorization"))
                return null;

            httpHeaders.TryGetValue("Authorization", out var authorizationString);

            string token = authorizationString.ToString().Split(' ')[1];
            return token;
        }
    }
}
