using System;
using System.Collections;
using System.Text;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    ///  A base class for an terminal command.
    /// </summary>
    public abstract class ServerCommand
    {
        private CommandContext _commandContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCommand"/> class.
        /// </summary>
        protected ServerCommand() { }

        /// <summary>
        /// A friendly discription for this command.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Get or sets the help system metadata.
        /// </summary>
        public ArrayList HelpCommands { get; set; } = new ArrayList();

        /// <summary>
        /// Gets or sets the command context.
        /// </summary>
        public CommandContext CommandContext
        {
            get
            {
                if (_commandContext == null)
                {
                    _commandContext = new CommandContext();
                }

                return _commandContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _commandContext = value;
            }
        }


        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">Information about the current request and action.</param>
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="context">Information about the current request and action.</param>
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="context">Information about the current request and action.</param>
        public virtual void OnException(ExceptionContext context) 
        { 
        }

        /// <summary>
        /// Provides interactive help for the <c>clear</c> terminal command.
        /// </summary>
        public virtual IActionResult Help()
        {
            var commandName = CommandContext.ActionDescriptor.CommandName;
            var actionName = CommandContext.ActionDescriptor.ActionName;

            HelpCommands.Add($"{commandName} {actionName}");
            
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine($" {commandName} [action] [--flags]");
            sb.AppendLine();
            sb.AppendLine("Available Commands:");
            
            foreach (string item in HelpCommands)
            {
                sb.AppendLine($" {item}");
            }
            sb.AppendLine();

            return new ResponseResult(sb.ToString()) { AppendLastLineFeed = false };
        }
    }
}