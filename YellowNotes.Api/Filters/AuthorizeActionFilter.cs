using Microsoft.AspNetCore.Mvc.Authorization;

namespace YellowNotes.Api.Filters
{
    public class AuthorizeActionFilter : AuthorizeFilter
    {
        public AuthorizeActionFilter() : base()
        {
        }
    }
}
