using System;

using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// A base class for an MVC controller.
    /// </summary>
    public abstract class Controller : ControllerBase, IController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        protected Controller() 
        { 
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

        internal virtual ActionResult TriggerOnException(Exception ex)
        {
            return null;
        }
    }
}