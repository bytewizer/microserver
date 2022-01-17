using System;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// A context for exception filters.
    /// </summary>
    public class ExceptionContext
    {
        /// <summary>
        /// Instantiates a new <see cref="ExceptionContext"/> instance.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/>.</param>
        /// <param name="exception">the <see cref="System.Exception"/> caught while executing the action.</param>
        public ExceptionContext(ActionContext actionContext, Exception exception)
        {
            HttpContext = actionContext.TerminalContext;
            Canceled = true;
            Exception = exception;
        }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public TerminalContext HttpContext { get; private set; }

        /// <summary>
        /// Gets or sets an indication that an action filter short-circuited the action and the action filter pipeline.
        /// </summary>
        public virtual bool Canceled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Exception"/> caught while executing the action.
        /// </summary>
        public virtual Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets an indication that the <see cref="Exception"/> has been handled.
        /// </summary>   
        public virtual bool ExceptionHandled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IActionResult"/>.
        /// </summary>
        public virtual IActionResult Result { get; set; }
    }
}
