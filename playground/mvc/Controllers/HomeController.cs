using System;

using Bytewizer.TinyCLR.Http.Mvc;

namespace Bytewizer.Playground.Mvc
{
    public class HomeController : Controller
    {
        // Any public IActionResult method inherited from Controller is made available as an endpoint
        public IActionResult Index()
        {    
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #00ace9 }h1 { font-size:3cm; text-align: center; color: #f6f6e8;}</style>" +
                "<meta http-equiv='refresh' content='5'></head><body><h1>" + $"{DateTime.Now.Ticks}" + "</h1></body></html>";

            return Content(response);
        }
    }
}