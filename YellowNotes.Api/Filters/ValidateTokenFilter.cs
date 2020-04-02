using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YellowNotes.Api.Extensions;
using YellowNotes.Core.Utility;

namespace YellowNotes.Api.Filters
{
    public class ValidateTokenFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAnonymousAllowed = context.ActionDescriptor.EndpointMetadata
                .Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
                
            if (isAnonymousAllowed)
            {
                return;
            }

            var userEmail = context.HttpContext.GetEmailFromClaims();
            if (userEmail == null)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Error: HttpContext.User.EmailClaim is null");
                return;
            }

            var isValidated = TokenUtility.Validate(userEmail,
                context.HttpContext.Request.Headers);

            if (!isValidated)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Error: Token is not valid!");
                return;
            }
        }
    }
}
