namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// A default implementation of <see cref="IActionResult"/>.
    /// </summary>
    public abstract class ActionResult : IActionResult
    {
        /// <summary>
        /// Executes the result operation of the action method synchronously. This method is called by MVC to process
        /// the result of an action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes
        /// information about the action that was executed and request information.</param>
        public abstract void ExecuteResult(ActionContext context);
    }
}
