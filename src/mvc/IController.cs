using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    public interface IController
    {
        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        void OnActionExecuting(ActionExecutingContext context);

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="context">The action executed context.</param>
        void OnActionExecuted(ActionExecutedContext context);

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="context">Information about the current request and action.</param>
        void OnException(ExceptionContext context);
    }
}
