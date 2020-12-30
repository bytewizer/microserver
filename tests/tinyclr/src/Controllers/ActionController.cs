using System.IO;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;

namespace Bytewizer.TinyCLR.TestHarness
{
    public class ActionController : Controller
    {
        public IActionResult GetBadRequest()
        {
            return new BadRequestResult();
        }

        public IActionResult GetBadRequest1()
        {
            return BadRequest();
        }

        public IActionResult GetContent()
        {
            var id = 0;
            string response = $"<!doctype html><title>{id}</title>";

            return new ContentResult(response)
            {
                ContentType = "text/html",
                StatusCode = StatusCodes.Status207MultiStatus
            };
        }

        public IActionResult GetContent1()
        {
            var id = 1;
            string response = $"<!doctype html><title>{id}</title>";

            return Content(response);
        }

        public IActionResult GetEmpty()
        {
            return new EmptyResult();
        }

        public IActionResult GetFile()
        {
            var fullPath = @"\img\wave.jpg";
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return File(stream, "image/jpeg", "ocean.jpeg");
        }

        public IActionResult GetFileStream()
        {
            var fullPath = @"\img\wave.jpg";
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new FileStreamResult(stream, "image/jpeg");
        }

        public IActionResult GetNotFound()
        {
            return new NotFoundResult();
        }

        public IActionResult GetNotFound1()
        {
            return NotFound();
        }

        public IActionResult GetOK()
        {
            return new OkResult();
        }

        public IActionResult GetOK1()
        {
            return Ok();
        }

        public IActionResult GetRedirect()
        {
            return new RedirectResult("action", "controller");
        }

        public IActionResult GetRedirect1()
        {
            return new RedirectResult("http://www.google.com");
        }

        public IActionResult GetRedirect2()
        {
            return Redirect("http://www.google.com");
        }

        public IActionResult GetStatusCode()
        {
            return new StatusCodeResult(StatusCodes.Status207MultiStatus);
        }

        public IActionResult GetStatusCode1()
        {
            return StatusCode(StatusCodes.Status302Found);
        }
    }
}