using System;

using Bytewizer.TinyCLR.Http.Mvc.ActionResults;

namespace Bytewizer.TinyCLR.Http.Mvc.Filters
{
    public class ActionExecutedContext
    {
        /// <summary>
        /// A context for action filters, specifically <see cref="OnActionExecuted"/> calls.
        /// </summary>
        public ActionExecutedContext(ActionContext actionContext)
        {
            HttpContext = actionContext.HttpContext;
        }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public HttpContext HttpContext { get; private set; }

        /// <summary>
        /// Gets or sets an indication that an action filter short-circuited the action and the action filter pipeline.
        /// </summary>
        public virtual bool Canceled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Exception"/> caught while executing the action or action filters, if any.
        /// </summary>
        public virtual Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets an indication that the <see cref="Exception"/> has been handled.
        /// </summary>
        public virtual bool ExceptionHandled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IActionResult"/>.
        /// </summary>
        public virtual IActionResult Result { get; set; } = new EmptyResult();
    }
}
