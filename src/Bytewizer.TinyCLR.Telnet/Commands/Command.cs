namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    ///  A base class for an telnet command.
    /// </summary>
    public abstract class Command : CommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        protected Command()
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