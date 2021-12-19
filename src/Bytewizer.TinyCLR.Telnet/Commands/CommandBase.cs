using System;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// A base class for a telnet command.
    /// </summary>
    public abstract class CommandBase
    {
        private CommandContext _commandContext;

        /// <summary>
        /// Gets or sets the <see cref="CommandContext"/>.
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
    }
}