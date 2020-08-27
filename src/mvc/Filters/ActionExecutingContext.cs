namespace Bytewizer.TinyCLR.Http.Mvc.Filters
{
    /// <summary>
    /// A context for action filters, specifically <see cref="OnActionExecuted"/> and
    /// <see cref="OnActionExecution"/> calls.
    /// </summary>
    public class ActionExecutingContext
    {
        /// <summary>
        /// Instantiates a new <see cref="ActionExecutingContext"/> instance.
        /// </summary>
        public ActionExecutingContext(ActionContext actionContext)
        {
            HttpContext = actionContext.HttpContext;
        }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public HttpContext HttpContext { get; private set;}

        /// <summary>
        /// Gets or sets the <see cref="IActionResult"/> to execute. Setting <see cref="Result"/> to a non-<c>null</c>
        /// value inside an action filter will short-circuit the action and any remaining action filters.
        /// </summary>
        public IActionResult Result { get; set; }
    }
}
