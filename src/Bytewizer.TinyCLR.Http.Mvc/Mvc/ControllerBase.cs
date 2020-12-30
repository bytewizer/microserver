using System;
using System.IO;

using Bytewizer.TinyCLR.Stubble;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    public abstract class ControllerBase
    {
        private ControllerContext _controllerContext;

        /// <summary>
        /// Gets the <see cref="Http.HttpContext"/> for the executing action.
        /// </summary>
        public HttpContext HttpContext => ControllerContext.HttpContext;

        /// <summary>
        /// Gets the <see cref="HttpRequest"/> for the executing action.
        /// </summary>
        public HttpRequest Request => HttpContext?.Request;

        /// <summary>
        /// Gets the <see cref="HttpResponse"/> for the executing action.
        /// </summary>
        public HttpResponse Response => HttpContext?.Response;

        /// <summary>
        /// Gets or sets the <see cref="ControllerContext"/>.
        /// </summary>
        public ControllerContext ControllerContext
        {
            get
            {
                if (_controllerContext == null)
                {
                    _controllerContext = new ControllerContext();
                }

                return _controllerContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _controllerContext = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ViewData"/> used by <see cref="ViewResult"/>.
        /// </summary>
        public ViewData ViewData { get; set; } = new ViewData();

        /// <summary>
        /// Creates a <see cref="ViewResult"/> object by specifying a <paramref name="viewName"/>.
        /// </summary>
        /// <param name="viewName">The name or path of the view that is rendered to the response.</param>
        public virtual ViewResult View(string viewName)
        {
            return new ViewResult()
            {
                ViewName = viewName,
                ViewData = ViewData
            };
        }

        /// <summary>
        /// Creates a <see cref="ViewPartial"/> object that renders a partial view to the response.
        /// </summary>
        public virtual ViewResult PartialView(string viewName, string viewSection)
        {
            return new ViewResult()
            {
                ViewName = viewName,
                ViewSection = viewSection,
                ViewData = ViewData
            };
        }

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object that serializes the specified <paramref name="data"/> object
        /// to JSON.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <returns>The created <see cref="JsonResult"/> that serializes the specified <paramref name="data"/>
        /// to JSON format for the response.</returns>
        public virtual JsonResult Json(object data)
        {
            return new JsonResult(data);
        }

        /// <summary>
        /// Creates a <see cref="StatusCodeResult"/> object by specifying a <paramref name="statusCode"/>.
        /// </summary>
        /// <param name="statusCode">The status code to set on the response.</param>
        /// <returns>The created <see cref="StatusCodeResult"/> object for the response.</returns>
        public virtual StatusCodeResult StatusCode(int statusCode)
            => new StatusCodeResult(statusCode);

        /// <summary>
        /// Creates a <see cref="ContentResult"/> object by specifying a <paramref name="content"/> string.
        /// </summary>
        /// <param name="content">The content to write to the response.</param>
        /// <returns>The created <see cref="ContentResult"/> object for the response.</returns>
        public virtual ContentResult Content(string content)
            => Content(content, "text/html");

        /// <summary>
        /// Creates a <see cref="ContentResult"/> object by specifying a
        /// <paramref name="content"/> string and a <paramref name="contentType"/>.
        /// </summary>
        /// <param name="content">The content to write to the response.</param>
        /// <param name="contentType">The content MIME type.</param>
        /// <returns>The created <see cref="ContentResult"/> object for the response.</returns>
        public virtual ContentResult Content(string content, string contentType)
        {
            var result = new ContentResult(content)
            {
                ContentType = contentType?.ToString()
            };

            return result;
        }

        /// <summary>
        /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="StatusCodes.Status200OK"/>) with the
        /// specified <paramref name="contentType" /> as the Content-Type and the
        /// specified <paramref name="fileDownloadName" /> as the suggested file name.
        /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
        /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The <see cref="Stream"/> with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <returns>The created <see cref="FileStreamResult"/> for the response.</returns>
        /// <remarks>
        /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
        /// </remarks>        
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName)
            => new FileStreamResult(fileStream, contentType) { FileDownloadName = fileDownloadName };

        /// <summary>
        /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="StatusCodes.Status200OK"/>) with the
        /// specified <paramref name="contentType" /> as the Content-Type and the
        /// specified <paramref name="fileDownloadName" /> as the suggested file name.
        /// This supports range requests (<see cref="StatusCodes.Status206PartialContent"/> or
        /// <see cref="StatusCodes.Status416RangeNotSatisfiable"/> if the range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The <see cref="Stream"/> with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
        /// <returns>The created <see cref="FileStreamResult"/> for the response.</returns>
        /// <remarks>
        /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
        /// </remarks>        
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, bool enableRangeProcessing)
            => new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            };

        /// <summary>
        /// Creates a <see cref="OkResult"/> object that produces an empty <see cref="StatusCodes.Status200OK"/> response.
        /// </summary>
        /// <returns>The created <see cref="OkResult"/> for the response.</returns>
        public virtual OkResult Ok()
            => new OkResult();

        /// <summary>
        /// Creates a <see cref="RedirectResult"/> object that redirects (<see cref="StatusCodes.Status302Found"/>)
        /// to the specified <paramref name="url"/>.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns>The created <see cref="RedirectResult"/> for the response.</returns>
        public virtual RedirectResult Redirect(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            return new RedirectResult(url);
        }

        /// <summary>
        /// Redirects (<see cref="StatusCodes.Status302Found"/>) to the specified action using the 
        /// <paramref name="actionName"/> and the <paramref name="controllerName"/>.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The created <see cref="RedirectResult"/> for the response.</returns>
        public virtual RedirectResult RedirectResult(string actionName, string controllerName)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException(nameof(controllerName));
            }

            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentException(nameof(actionName));
            }

            return new RedirectResult(controllerName, actionName);
        }

        /// <summary>
        /// Creates an <see cref="BadRequestResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/> response.
        /// </summary>
        /// <returns>The created <see cref="BadRequestResult"/> for the response.</returns>
        public virtual BadRequestResult BadRequest()
            => new BadRequestResult();

        /// <summary>
        /// Creates an <see cref="NotFoundResult"/> that produces a <see cref="StatusCodes.Status404NotFound"/> response.
        /// </summary>
        /// <returns>The created <see cref="NotFoundResult"/> for the response.</returns>
        public virtual NotFoundResult NotFound()
            => new NotFoundResult();
    }
}
