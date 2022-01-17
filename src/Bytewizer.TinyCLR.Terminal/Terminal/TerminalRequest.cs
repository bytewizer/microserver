namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents the incoming side of an individual terminal request.
    /// </summary>
    public class TerminalRequest
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TerminalRequest" /> class.
        /// </summary>
        public TerminalRequest()
        {
        }

        /// <summary>
        /// Gets the Terminal command to be executed.
        /// </summary>
        public CommandLine Command { get; set; }


        /// <summary>
        /// Clears the <see cref="TerminalRequest"/> object.
        /// </summary>
        public void Clear()
        {
            Command?.Clear();
        }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        public override string ToString()
        {
            return $"{Command}";
        }
    }
}