using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.TestHarness
{
    public class CircutController : Controller
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;

            filterContext.Canceled = true; // this prevents the action from finishing
            filterContext.Result = new ContentResult($"A short circut occurred in the {actionName} action.")
            {
                ContentType = "text/plain",
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        public IActionResult Short()
        {
            return Content("Short circut this action controller");                
        }
    }
}