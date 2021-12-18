namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Defines a contract that represents the result of a command action method.
    /// </summary>
    public interface IActionResult
    {
        /// <summary>
        /// Executes the result operation of the command action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed.</param>
        void ExecuteResult(ActionContext context);
    }
}
