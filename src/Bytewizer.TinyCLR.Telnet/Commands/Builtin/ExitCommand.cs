using Bytewizer.TinyCLR.Telnet;

namespace Bytewizer.Playground.Telnet.Commands
{
    /// <summary>
    /// Implements the <c>exit</c> telnet command.
    /// </summary>
    public class ExitCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommand"/> class.
        /// </summary>
        public ExitCommand()
        {
            Description = "Disconnects the client by closing the connection";
        }

        /// <summary>
        /// Disconnects the client by closing the connection.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {
            CommandContext.TelnetContext.Active = false;
            
            return new EmptyResult();
        }
    }
}