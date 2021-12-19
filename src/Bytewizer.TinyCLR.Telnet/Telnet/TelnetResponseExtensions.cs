namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Convenience methods for <see cref="TelnetResponse"/>.
    /// </summary>
    public static class TelnetResponseExtensions
    {
        /// <summary>
        /// Writes the given control message to the response.
        /// </summary>
        /// <param name="response">The <see cref="TelnetResponse"/>.</param>
        /// <param name="message">The control message to write to the response.</param>
        public static void Write(this TelnetResponse response, string message)
        {
            response.Message = message;
        }

        /// <summary>
        /// Writes the given control message to the response.
        /// </summary>
        /// <param name="response">The <see cref="TelnetResponse"/>.</param>
        /// <param name="message">The control message to write to the response.</param>
        public static void WriteLine(this TelnetResponse response, string message)
        {
            response.Message = message + "\r\n\r\n";
        }
    }
}