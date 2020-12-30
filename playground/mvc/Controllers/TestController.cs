using System.IO;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;

namespace Bytewizer.Playground.Mvc
{
    // Any public IActionResult method inherited from Controller is made available as an endpoint
    public class TestController : Controller
    {
        public IActionResult GetById(int id)
        { 
            
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{id}" + "</h1></body></html>";

            return Content(response, "text/html");
        }

        public IActionResult GetByName(string name)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + $"{name}" + "</h1></body></html>";

            return Content(response, "text/html");
        }

        public IActionResult GetOK()
        {
            return Ok();
        }

        public IActionResult GetBadRequest()
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

        public IActionResult GetFile()
        {
            var fullPath = @"\img\ocean.jpg";
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            return File(stream, "image/jpeg", "ocean.jpeg");
        }

        public IActionResult PostForm(string firstname, string lastname)
        {
            var fullname = $"{ firstname } { lastname }";

            return Content(fullname, "text/html");
        }

        public IActionResult PostForm2()
        {
            var body = HttpContext.Request.Body;

            var reader = new StreamReader(body);

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                Debug.WriteLine(line);
            }

            return Ok();
        }
    }

    public class JsonData
    {
        public JsonData()
        {
            Name = "Bob Smith";
            Address = "103 Main Street, Burbank, CA 92607";
        }

        public string Name { get; set; }
        public string Address { get; set; }
    }
}