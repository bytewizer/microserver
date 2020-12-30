using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    ///  A base class for an MVC controller with view support.
    /// </summary>
    public abstract class Controller : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        protected Controller()
        {
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