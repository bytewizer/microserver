using System;

namespace Bytewizer.TinyCLR.Http.Mvc.Filters
{
    /// <summary>
    /// A context for action filters.
    /// </summary>
    public class ActionExecutedContext : FilterContext
    {
        /// <summary>
        /// Instantiates a new <see cref="ActionExecutingContext"/> instance.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/>.</param>
        /// <param name="controller">The controller instance containing the action.</param>
        public ActionExecutedContext(ActionContext actionContext, object controller)
            : base(actionContext)
        {
            Controller = controller;
        }

        /// <summary>
        /// Gets or sets an indication that an action filter short-circuited the action and the action filter pipeline.
        /// </summary>
        public virtual bool Canceled { get; set; }

        /// <summary>
        /// Gets the controller instance containing the action.
        /// </summary>
        public virtual object Controller { get; }

        /// <summary>
        /// Gets or sets the <see cref="System.Exception"/> caught while executing the action or action filters, if any.
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
