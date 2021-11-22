using System;
using System.Text;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// FTP command with argument.
    /// </summary>
    public sealed class FtpCommand
    {
        private static readonly char[] _whiteSpaces = { ' ', '\t' };

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpCommand"/> class.
        /// </summary>
        public FtpCommand() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpCommand"/> class.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="commandArgument">The command argument.</param>
        public FtpCommand(string commandName, string commandArgument)
        {
            Name = commandName.Replace(Environment.NewLine, string.Empty).ToUpper();
            Argument = commandArgument.Replace(Environment.NewLine, string.Empty) ?? string.Empty;
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the command argument.
        /// </summary>
        public string Argument { get; private set; }

        /// <summary>
        /// Splits the <paramref name="command"/> into the name and its arguments.
        /// </summary>
        /// <param name="command">The command to split into name and arguments.</param>
        /// <returns>The created <see cref="FtpCommand"/>.</returns>
        public static FtpCommand Parse(byte[] buffer, int offset, int count)
        {
            var command = Encoding.UTF8.GetString(buffer, 0, count);
            var spaceIndex = command.IndexOfAny(_whiteSpaces);
            var commandName = spaceIndex == -1 ? command : command.Substring(0, spaceIndex);
            var commandArguments = spaceIndex == -1 ? string.Empty : command.Substring(spaceIndex + 1);
            return new FtpCommand(commandName, commandArguments);
        }

        public override string ToString()
        {
            var message =
                Name.StartsWith("PASS")
                    ? "PASS **************** (password omitted)"
                    : $"{Name} {Argument}";
            return message;
        }

        /// <summary>
        /// Clears the <see cref="FtpCommand"/> name and arguments.
        /// </summary>
        public void Clear()
        {
            Name = default;
            Argument = default;
        }
    }
}