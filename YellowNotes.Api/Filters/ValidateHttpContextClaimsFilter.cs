using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YellowNotes.Api.Extensions;

namespace YellowNotes.Api.Filters
{
    public class ValidateHttpContextClaimsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userEmail = context.HttpContext.GetEmailFromClaims();
            if (userEmail != null)
            {
                return;
            }

            context.Result = new BadRequestObjectResult(
                "Error: HttpContext.User.EmailClaim is null");
        }
    }
}
