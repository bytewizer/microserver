using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.WebServer
{
    public class ErrorController : Controller
    {     
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // called before action method    
            if (filterContext.HttpContext.Request.Method != HttpMethods.Get)
            {
                filterContext.Result = new BadRequestResult();
            }
        }

        public override void OnException(ExceptionContext filterContext)
        {
            // called on action method execption
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ContentResult
            {
                Content = $"An error occurred in the {actionName} action.",
                ContentType = "text/plain",
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public IActionResult Throw()
        {
            throw new ArgumentNullException("thowing server error");
        }
    }
}
