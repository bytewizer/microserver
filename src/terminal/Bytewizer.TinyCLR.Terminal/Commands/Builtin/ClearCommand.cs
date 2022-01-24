using System.Collections;

namespace Bytewizer.TinyCLR.Terminal.Commands
{
    /// <summary>
    /// Implements the <c>clear</c> terminal command.
    /// </summary>
    public class ClearCommand : ServerCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearCommand"/> class.
        /// </summary>
        public ClearCommand()
        {
            Description = "Clears the screen for the connected session";
            HelpCommands = new ArrayList()
            {
                { "clear" }
            };
        }

        /// <summary>
        /// Clears the screen for the connected client.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {            
            return new ClearResult();
        }
    }
}