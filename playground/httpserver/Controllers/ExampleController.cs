using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.WebServer.Controllers
{
    public class ExampleController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // called before action method
            if (filterContext.HttpContext.Request.Method != HttpMethods.Get)
            {
                filterContext.Result = new BadRequestResult();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // called after action method
        }

        public override void OnException(ExceptionContext filterContext)
        {
            // called on action method execption
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            filterContext.Result = new ContentResult
            {
                Content = $"An error occurred in the {actionName} action.",
                ContentType = "text/plain",
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        // Any public IActionResult method inherited from Controller is made available as an endpoint
        public IActionResult GetById(long id)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{id}" + "</h1></body></html>";

            return Content(response, "text/html");
        }
    }
}
