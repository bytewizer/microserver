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
        /// Initializes a new instance of the <see cref="HelpCommand"/> class.
        /// </summary>
        public HelpCommand()
        {
            Description = "Description of the interactive help system";
        }

        /// <summary>
        /// Description of the interactive help system.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine(" command [action] [--flags]");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Available Commands:");
            sb.AppendLine(string.Empty);
            foreach (DictionaryEntry item in CommandContext.GetCommands())
            {
                sb.AppendLine(string.Format(" {0}          {1}", item.Key.ToString(), item.Value.ToString()));
            }

            return Response(sb.ToString());
        }
    }
}