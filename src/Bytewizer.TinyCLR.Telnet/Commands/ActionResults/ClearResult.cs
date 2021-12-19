using System;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// An action result which cleares the screen to the client. 
    /// </summary>
    public class ClearResult : ActionResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearResult"/> class.
        /// </summary>
        public ClearResult()
        {
        }

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.TelnetContext.Response.Write("\u001B[1J\u001B[H");     
        }
    }
}