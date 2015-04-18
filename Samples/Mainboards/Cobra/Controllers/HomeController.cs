using System;
using System.IO;
using System.Text;

using Microsoft.SPOT;
using Microsoft.SPOT.IO;

using MicroServer.Net.Http.Mvc;
using MicroServer.Serializers.Json;


namespace MicroServer.CobraII
{
    public class HomeController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }

        public IActionResult GetHello(ControllerContext context)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + DateTime.Now.ToString() + "</h1></body></html>\r\n";

            return ContentResult(response, "text/html");
        }
    }
}
