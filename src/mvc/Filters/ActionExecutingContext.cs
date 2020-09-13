using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.Filters
{
    /// <summary>
    /// A context for action filters, specifically <see cref="IActionFilter.OnActionExecuted"/>
    /// </summary>
    public class ActionExecutingContext : FilterContext
    {
        /// <summary>
        /// Instantiates a new <see cref="ActionExecutingContext"/> instance.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/>.</param>
        /// <param name="filters">All applicable <see cref="IFilterMetadata"/> implementations.</param>
        /// <param name="actionArguments">
        /// The arguments to pass when invoking the action. Keys are parameter names.
        /// </param>
        /// <param name="controller">The controller instance containing the action.</param>
        public ActionExecutingContext(ActionContext actionContext, object controller)
            : base(actionContext)
        {
            Controller = controller;
        }

        /// <summary>
        /// Gets or sets the <see cref="IActionResult"/> to execute. Setting <see cref="Result"/> to a non-<c>null</c>
        /// value inside an action filter will short-circuit the action and any remaining action filters.
        /// </summary>
        public virtual IActionResult Result { get; set; }

        /// <summary>
        /// Gets the controller instance containing the action.
        /// </summary>
        public virtual object Controller { get; }
    }
}
