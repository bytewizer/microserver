using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Hardware;
using Bytewizer.TinyCLR.Http.Mvc;

namespace Bytewizer.Playground.AspNet
{
    // Any public IActionResult method inherited from Controller is made available as an endpoint
    public class HomeController : Controller
    {
        //private readonly ILogger _logger;
        //private readonly IHardware _hardware;

        public HomeController()
        {
        }

        //public HomeController(ILoggerFactory loggerFactory, IHardware hardware)
        //{
        //    _logger = loggerFactory.CreateLogger(typeof(HomeController));
        //    _hardware = hardware;
        //}

        public IActionResult GetOK()
        {            
            //_logger.LogInformation("GetOK() is working.");
            return Ok();
        }

        public IActionResult GetById(int id)
        {
            var context = HttpContext.Request.RouteValues;

            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{id}" + "</h1></body></html>";

            return Content(response);
        }
    }
}