namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Common ANSI sequences to simplify construction of correct ANSI for adherance to the telnet protocol.</summary>
    public class AnsiSequences
    {
        /// <summary>
        /// Gets the ANSI sequence to move the cursor to a new line.
        /// </summary>
        /// <remarks>Explicitly avoids Environment.NewLine, as a Telnet server is always supposed to send new lines as CR LF.</remarks>
        public const string NewLine = "\r\n";

        /// <summary>
        /// The ANSI 'escape sequence'.
        /// </summary>
        public const string Esc = "\x1B";
    }
}
