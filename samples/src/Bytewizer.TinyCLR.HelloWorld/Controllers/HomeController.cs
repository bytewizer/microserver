using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Filters;

using GHIElectronics.TinyCLR.Native;

namespace Bytewizer.TinyCLR.HelloWorld
{
    public class HomeController : Controller
    {
        private readonly string version;
        private readonly string deviceName;
        private readonly string manufacturerName;

        public HomeController()
        {
            manufacturerName = DeviceInformation.ManufacturerName;
            deviceName = DeviceInformation.DeviceName;

            var major = (ushort)((DeviceInformation.Version >> 48) & 0xFFFF);
            var minor = (ushort)((DeviceInformation.Version >> 32) & 0xFFFF);
            var build = (ushort)((DeviceInformation.Version >> 16) & 0xFFFF);
            var revision = (ushort)((DeviceInformation.Version >> 0) & 0xFFFF);
            version = $"{major}.{minor}.{build}.{revision}";
        }

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
            filterContext.Result = new ContentResult($"An error occurred in the {actionName} action.")
            {
                ContentType = "text/plain",
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public IActionResult Index()
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title><style>" +
                "body{font-size: .7cm;background-color:#00203FFF;color:#ADEFD1FF}" +
                "h1{font-size:2cm;text-align:center;color:#ADEFD1FF}h2{font-size:1cm;text-align:center;color:#ADEFD1FF}" +
                "table{border-collapse:collapse;margin:0;padding:0;width:100%;text-align:center;color:#ADEFD1FF;table-layout:fixed}" +
                "a:link{color:#ADEFD1FF}a:visited{color:#ADEFD1FF}a:link{text-decoration:none}</style></head>" +
                $"<body><h1>{manufacturerName}</h1><h2>Device: {deviceName} Version: {version}</h2>" +
                "<table><tbody><tr><td><a href='persons'>Get JSON</a></td></tr></tbody><tbody><tr><td>" +
                "<a href='ticks'>Get Ticks</a></td></tr></tbody><tbody><tr><td><a href='home/throw'>" +
                "Throw an Error</a></td></tr></tbody></table></body></html>";

            return Content(response);
        }

        public IActionResult Throw()
        {
            throw new ArgumentNullException("Thowing server error");
        }
    }
}
