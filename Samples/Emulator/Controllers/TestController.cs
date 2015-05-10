using System;
using System.IO;
using System.Text;

using Microsoft.SPOT;
using Microsoft.SPOT.IO;

using MicroServer.Net.Http.Mvc;
using MicroServer.Serializers.Json;

namespace MicroServer.Emulator
{
    public class TestController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.Print("Emulator => OnActionExecuting");

            if (!filterContext.HttpContext.Request.HttpMethod.ToUpper().Equals("GET"))
                filterContext.Result = EmptyResult();

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Debug.Print("Emulator => OnActionExecuted > " + filterContext.HttpContext.ActionName);

            if (filterContext.Exception != null)
                Debug.Print(filterContext.HttpContext.ControllerName + " exception was thrown");

            base.OnActionExecuted(filterContext);
        }

        public override void OnException(ExceptionContext filterContext)
        {
            Debug.Print("Emulator => OnException");

            filterContext.ExceptionHandled = true;
            filterContext.Result = new HttpStatusCodeResult(500," Internal Server Error");

            base.OnException(filterContext);
        }

        public IActionResult GetEmpty(ControllerContext context)
        {
            Debug.Print(context.ControllerName);
            
            Debug.Print("Emulator => GetEmpty");

            return EmptyResult();
        }

        public IActionResult GetHello(ControllerContext context)
        {
            Debug.Print("Emulator => GetHello");

            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + DateTime.Now.ToString() + "</h1></body></html>\r\n";

            return ContentResult(response, "text/html");
        }

        public IActionResult GetJson(ControllerContext context)
        {

            Debug.Print("Emulator => GetJson");

            JArray jcar = new JArray();
            jcar.Add(new JProperty("make", "ford"));
            jcar.Add(new JProperty("model", "mustang"));
            jcar.Add(new JProperty("color", "blue"));

            JArray jcar1 = new JArray();
            jcar1.Add(new JProperty("make", "bmw"));
            jcar1.Add(new JProperty("model", "M5"));
            jcar1.Add(new JProperty("color", "black"));

            JArray jgroup = new JArray();
            jgroup.Add(new JProperty("group", "car"));
            jgroup.Add(new JProperty("type", jcar));
            jgroup.Add(new JProperty("type", jcar1));

            JArray jar = new JArray();
            jar.Add(new JProperty("style", ""));
            jar.Add(new JProperty("data-role", "update"));
            jar.Add(new JProperty("data-inset", "true"));
            jar.Add(new JProperty("group", jgroup));

            JObject jdom = new JObject();
            jdom.Add("date", new DateTime(1983, 3, 20).ToString());
            jdom.Add("data-role", "update");
            jdom.Add("style", jar);

            return JsonResult(jdom);
        }

        public IActionResult GetRedirect(ControllerContext context)
        {
            return RedirectResult("test", "getjson");
        }

        public IActionResult GetGoogle(ControllerContext context)
        {
            Debug.Print("Emulator => GetGoogle");

            return RedirectResult("http://www.google.com/");
        }

        public IActionResult GetExecption(ControllerContext context)
        {
            throw new NullReferenceException("Testing Null Reference Exception");
        }

        public IActionResult GetError(ControllerContext context)
        {
            Debug.Print("Emulator => GetError");

            return new HttpStatusCodeResult(404, "Page Not Found");
        }
    }
}
