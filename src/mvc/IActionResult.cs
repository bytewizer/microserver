namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// Defines a contract that represents the result of an action method.
    /// </summary>
    public interface IActionResult
    {
        /// <summary>
        /// Executes the result operation of the action method. This method is called by MVC to process
        /// the result of an action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes
        /// information about the action that was executed and request information.</param>
        void ExecuteResult(ActionContext context);
    }
}
