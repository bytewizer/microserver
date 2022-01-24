using System;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// An action result which cleares the screen to the client. 
    /// </summary>
    public class ClearResult : ActionResult
    {

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.TerminalContext.Response.Write("\u001B[1J\u001B[H");     
        }
    }
}