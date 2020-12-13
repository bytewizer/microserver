using Bytewizer.TinyCLR.Http.Mvc;

namespace Bytewizer.Playground.Hosting
{
    // Any public IActionResult method inherited from Controller is made available as an endpoint
    public class HomeController : Controller
    {
        public IActionResult GetOK()
        {
            return Ok();
        }

        public IActionResult GetById(int id)
        { 
            
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{id}" + "</h1></body></html>";

            return Content(response, "text/html");
        }
    }
}