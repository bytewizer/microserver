using System.Collections;

namespace Bytewizer.TinyCLR.Terminal.Commands
{
    /// <summary>
    /// Implements the <c>whoami</c> terminal command.
    /// </summary>
    public class WhoamiCommand : ServerCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhoamiCommand"/> class.
        /// </summary>
        public WhoamiCommand()
        {
            Description = "Display the user name associated with the current session";
            HelpCommands = new ArrayList()
            {
                { "whoami" }
            };
        }

        /// <summary>
        /// Disconnects the client by closing the connection.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {            
            var username = CommandContext.GetUserName();
            return new ResponseResult(username);
        }
    }
}