using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace YellowNotes.Core.Utility
{
    public class ResultHandler
    {
        public HttpStatusCode StatusCode { get; }

        public object Payload { get; }

        public ResultHandler(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public ResultHandler(HttpStatusCode statusCode, object payload)
        {
            StatusCode = statusCode;
            Payload = payload;
        }

        public IActionResult GetActionResult(ControllerBase controller)
        {
            if (Payload != null)
            {
                return controller.StatusCode((int)StatusCode, Payload);
            }
            return controller.StatusCode((int)StatusCode);
        }
    }
}
