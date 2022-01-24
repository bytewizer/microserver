using System.Text;
using System.Collections;

namespace Bytewizer.TinyCLR.Terminal.Commands
{
    /// <summary>
    /// Implements the <c>help</c> terminal command.
    /// </summary>
    public class HelpCommand : ServerCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpCommand"/> class.
        /// </summary>
        public HelpCommand()
        {
            Description = "Retrieve basic help including a list of commands";
            HelpCommands = new ArrayList()
            {
                { "help" }
            };
        }

        /// <summary>
        /// Description of the interactive help system.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {
            return Help();
        }

        /// <summary>
        /// Description of the interactive help system.  This is the default action.
        /// </summary>
        public override IActionResult Help()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine(" command [action] [parm(s)] [--flags]");
            sb.AppendLine();
            sb.AppendLine("Available Commands:");

            foreach (DictionaryEntry item in CommandContext.GetAvailableCommands())
            {
                sb.AppendLine($" {item.Key,-14} {item.Value}");
            }
            sb.AppendLine();

            return new ResponseResult(sb.ToString()) { AppendLastLineFeed = false};
        }
    }
}