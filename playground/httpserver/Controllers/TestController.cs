using System;
using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Filters;
using GHIElectronics.TinyCLR.Data.Json;

namespace Bytewizer.TinyCLR.WebServer.Controllers
{
    public class TestController : Controller
    {
        // Any public IActionResult method inherited from Controller is made available as an endpoint
        public IActionResult GetById(long id)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{id}" + "</h1></body></html>\r\n";

            return Content(response, "text/html");
        }

        public IActionResult GetByName(string name)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{name}" + "</h1></body></html>\r\n";

            return Content(response, "text/html");
        }

        public IActionResult GetOK()
        {
            return Ok();
        }

        public IActionResult GetRequest()
        {
            return BadRequest();
        }

        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        public IActionResult GetStatusCode()
        {
            return StatusCode(StatusCodes.Status207MultiStatus);
        }

        public IActionResult GetRedirect()
        {
            return Redirect("http://www.google.com");
        }

        public IActionResult GetJson()
        {
            var jsonObject = new JsonData();

            return Json(jsonObject);
        }
    }

    public class JsonData
    {
        public JsonData()
        {
            Name = "Bob Smith";
            Address = "103 Main Street, Burbank, CA 92607";
        }

        public string Name { get; set;}
        public string Address { get; set; }
    }
}
