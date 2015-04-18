using System;
using System.IO;
using System.Text;

using Microsoft.SPOT;

using MicroServer.Net.Http.Mvc;
using MicroServer.Serializers.Json;


namespace MicroServer.Emulator
{
    public class TestController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.Print("Test Harness => OnActionExecuting");

            if (!filterContext.HttpContext.Request.HttpMethod.ToUpper().Equals("GET"))
                filterContext.Result = EmptyResult();

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Debug.Print("Test Harness => OnActionExecuted > " + filterContext.HttpContext.ActionName);

            if (filterContext.Exception != null)
                Debug.Print(filterContext.HttpContext.ControllerName + " exception was thrown");

            base.OnActionExecuted(filterContext);
        }

        public override void OnException(ExceptionContext filterContext)
        {
            Debug.Print("Test Harness => OnException");

            filterContext.ExceptionHandled = true;
            filterContext.Result = new HttpStatusCodeResult(500," Internal Server Error");

            base.OnException(filterContext);
        }

        public IActionResult GetEmpty(ControllerContext context)
        {
            Debug.Print(context.ControllerName);
            return EmptyResult();
        }

        public IActionResult GetError(ControllerContext context)
        {
            Debug.Print("Test Harness => GetError");

            return new HttpStatusCodeResult(404, "Page Not Found");
        }
    }
}
