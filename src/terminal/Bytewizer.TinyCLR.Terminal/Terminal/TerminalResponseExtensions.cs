namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Convenience methods for <see cref="TerminalResponse"/>.
    /// </summary>
    public static class TerminalResponseExtensions
    {
        /// <summary>
        /// Writes the given control message to the response.
        /// </summary>
        /// <param name="response">The <see cref="TerminalResponse"/>.</param>
        /// <param name="message">The control message to write to the response.</param>
        public static void Write(this TerminalResponse response, string message)
        {
            response.Message = message;
        }

        /// <summary>
        /// Writes the given control message to the response.
        /// </summary>
        /// <param name="response">The <see cref="TerminalResponse"/>.</param>
        /// <param name="message">The control message to write to the response.</param>
        public static void WriteLine(this TerminalResponse response, string message)
        {
            response.Message = message + AnsiSequences.NewLine;
        }

        /// <summary>
        /// Writes the given control command message to the response.
        /// </summary>
        /// <param name="response">The <see cref="TerminalResponse"/>.</param>
        /// <param name="name1">The control name data to write to the response.</param>
        /// <param name="name2">The control name data to write to the response.</param>
        /// <param name="code">The control option data to write to the response.</param>
        public static void WriteCommand(this TerminalResponse response, char name1, char name2, char code)
        {
            response.Message = new string(new char[] {name1, name2, code} );
        }
    }
}