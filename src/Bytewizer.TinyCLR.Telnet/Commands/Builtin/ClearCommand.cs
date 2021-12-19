using System.Text;

using Bytewizer.TinyCLR.Telnet;

namespace Bytewizer.Playground.Telnet.Commands
{
    /// <summary>
    /// Implements the <c>clear</c> telnet command.
    /// </summary>
    public class ClearCommand : Command
    {
        /// <summary>
        /// Clears the screen for the connected client.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {            
            return new ClearResult();
        }

        /// <summary>
        /// Provides interactive help for the <c>clear</c> telnet command.
        /// </summary>
        public IActionResult Help()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Available Commands:");
            sb.AppendLine();
            sb.AppendLine(" clear");
            sb.AppendLine();

            return new ResponseResult(sb.ToString()) { NewLine = false };
        }
    }
}