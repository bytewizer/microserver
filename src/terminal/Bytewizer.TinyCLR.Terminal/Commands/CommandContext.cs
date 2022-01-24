using System;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// The context associated with the current request for a command.
    /// </summary>
    public class CommandContext : ActionContext
    {
        /// <summary>
        /// Creates a new <see cref="CommandContext"/>.
        /// </summary>
        public CommandContext() { }

        /// <summary>
        /// Creates a new <see cref="CommandContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="ActionContext"/> associated with the current request.</param>
        public CommandContext(ActionContext context)
            : base(context)
        {
            if (!(context.ActionDescriptor is CommandActionDescriptor))
            {
                throw new ArgumentException(nameof(context));
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CommandActionDescriptor"/> associated with the current request.
        /// </summary>
        public new CommandActionDescriptor ActionDescriptor
        {
            get { return (CommandActionDescriptor)base.ActionDescriptor; }
            set { base.ActionDescriptor = value; }
        }
    }
}