using Bytewizer.TinyCLR.Stubble;
using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// A base class for an MVC controller.
    /// </summary>
    public abstract class Controller : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        protected Controller()
        {
        }

        /// <summary>
        /// Gets or sets <see cref="ViewData"/> used by <see cref="ViewResult"/>.
        /// </summary>
        public ViewData ViewData { get; set; } = new ViewData();

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
        /// Creates a <see cref="PartialViewResult"/> object that renders a partial view to the response.
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

        /// <inheritdoc />
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
        }

        /// <inheritdoc />
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <inheritdoc />
        public virtual void OnException(ExceptionContext context) 
        { 
        }
    }
}