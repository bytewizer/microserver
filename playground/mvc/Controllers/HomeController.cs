using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.Playground.Mvc.Controllers
{
    public class HomeController : Controller
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
            filterContext.Result = new ContentResult($"An error occurred in the {actionName} action.")
            { 
                ContentType = "text/plain",
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        // Any public IActionResult method inherited from Controller is made available as an endpoint
        public IActionResult Index()
        {
      
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{DateTime.Now.Ticks}" + "</h1></body></html>";

            return Content(response);
        }
    }
}