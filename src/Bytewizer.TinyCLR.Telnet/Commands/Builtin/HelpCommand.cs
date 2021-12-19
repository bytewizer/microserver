using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Telnet;

namespace Bytewizer.Playground.Telnet.Commands
{
    /// <summary>
    /// Implements the <c>help</c> telnet command
    /// </summary>
    public class HelpCommand : Command
    {
        /// <summary>
        /// Description of the interactive help system.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine(" command [action] [--flags]");
            sb.AppendLine();
            sb.AppendLine("Available Commands:");
            sb.AppendLine();
            foreach (DictionaryEntry item in CommandContext.GetCommands())
            {
                sb.AppendLine(item.Key.ToString());
            }
            //sb.AppendLine("Description of the interactive help system");
            sb.AppendLine();

            return new ResponseResult(sb.ToString()) { NewLine = false};
        }
    }
}