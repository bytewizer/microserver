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
        /// Gets or sets friendly description for this command.
        /// </summary>
        public string Description { get; set; }

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

        /// <summary>
        /// Creates a <see cref="ResponseResult"/> object by specifying a <paramref name="content"/> string.
        /// </summary>
        /// <param name="content">The content to write to the response.</param>
        public virtual ResponseResult Response(string content)
        {
            return new ResponseResult(content);
        }
    }
}