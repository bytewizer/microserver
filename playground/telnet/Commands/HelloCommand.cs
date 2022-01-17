using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Terminal;
using Bytewizer.TinyCLR.Terminal.Ansi;

namespace Bytewizer.Playground.Terminal.Commands
{
    /// <summary>
    /// Implements the <c>hello</c> terminal command.
    /// </summary>
    public class HelloCommand : ServerCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelloCommand"/> class.
        /// </summary>
        public HelloCommand()
        {
            Description = "Hello world showing off ansi attributes command arguments";
            HelpCommands = new ArrayList()
            {
                { "hello" },
                { "hello world [parm1] [parm2] [--arguments]" }
            };
        }

        public override void OnException(ExceptionContext filterContext)
        {
            // called on action method execption
            filterContext.ExceptionHandled = false;
            filterContext.Result = new ResponseResult(filterContext.Exception.Message);
        }

        public IActionResult Default()
        {
            var sb = new StringBuilder();

            sb.AppendAnsi(AnsiTextAttribute.Bold, AnsiForegroundColor.Green, AnsiBackgroundColor.Blue);
            sb.Append("Hello from terminal server!");
            sb.AppendAnsi(AnsiTextAttribute.Normal);

            return new ResponseResult(sb);
        }

        public IActionResult World(string parm1, double parm2)
        {
            var sb = new StringBuilder();

            sb.AppendAnsi(AnsiTextAttribute.Bold, AnsiForegroundColor.Green, AnsiBackgroundColor.Blue);
            sb.Append($"Hello from terminal server! {parm1} {parm2}");
            foreach (DictionaryEntry argument in CommandContext.Arguments)
            {
                sb.Append($" {argument.Key}={argument.Value}");
            }
            sb.AppendAnsi(AnsiTextAttribute.Normal);

            return new ResponseResult(sb);
        }
    }
}