using System;
using System.Collections;
using System.Text;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Telnet command with argument.
    /// </summary>
    public sealed class TelnetCommand
    {
        private static readonly char[] _whiteSpaces = { ' ', '\t' };

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetCommand"/> class.
        /// </summary>
        public TelnetCommand() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetCommand"/> class.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <param name="action">The command action.</param>
        /// <param name="arguments">The command arguments.</param>
        public TelnetCommand(string name, string action, ArgumentCollection arguments)
        {
            Name = name.Replace(Environment.NewLine, string.Empty);
            Action = action.Replace(Environment.NewLine, string.Empty);
            Arguments = arguments;
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the command action.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Gets the command arguments object.
        /// </summary>
        public ArgumentCollection Arguments { get; private set; }

        /// <summary>
        /// Splits the <paramref name="buffer"/> into the name and its arguments.
        /// </summary>
        /// <param name="buffer">The command to split into name and arguments.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="count">The number of bytes to parse.</param>
        public static TelnetCommand Parse(byte[] buffer, int offset, int count)
        {
            var command = Encoding.UTF8.GetString(buffer, offset, count).Replace(Environment.NewLine, string.Empty);

            if (string.IsNullOrEmpty(command.Trim()))
            {
                return null;    
            }

            var parts = command.Split(new char[] { ' ' });
            var commandName = parts[0].Trim().ToLower();
            var commandAction = parts.Length > 1 ? parts[1].Trim().ToLower() : "default";

            var argsList = new ArrayList(parts);
            var commandArguments = new ArgumentCollection(ArgumentParser.ParseArguments(argsList));

            return new TelnetCommand(commandName, commandAction, commandArguments);
        }

        public override string ToString()
        {
            return $"{Name}/{Action}";
        }

        /// <summary>
        /// Clears the <see cref="TelnetCommand"/> name and arguments.
        /// </summary>
        public void Clear()
        {
            Name = default;
            Action = default;
            Arguments = default;
        }
    }
}