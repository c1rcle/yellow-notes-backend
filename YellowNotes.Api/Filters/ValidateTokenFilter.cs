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
            var userEmail = context.HttpContext.GetEmailFromClaims();

            var controller = context.Controller as ControllerBase;
            var errorMessage = TokenUtility.Validate(userEmail, controller.Request.Headers);
            if (errorMessage == null)
            {
                return;
            }

            context.Result = new UnauthorizedObjectResult(errorMessage);
        }
    }
}
