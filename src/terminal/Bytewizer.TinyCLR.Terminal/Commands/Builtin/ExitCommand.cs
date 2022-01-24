using System.Collections;

namespace Bytewizer.TinyCLR.Terminal.Commands
{
    /// <summary>
    /// Implements the <c>exit</c> terminal command.
    /// </summary>
    public class ExitCommand : ServerCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommand"/> class.
        /// </summary>
        public ExitCommand()
        {
            Description = "Close open session and exit";
            HelpCommands = new ArrayList()
            {
                { "exit" }
            };
        }

        /// <summary>
        /// Disconnects the client by closing the connection.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {
            CommandContext.TerminalContext.Active = false;
            
            return new EmptyResult();
        }
    }
}