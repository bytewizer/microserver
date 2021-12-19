namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Represents the incoming side of an individual telnet request.
    /// </summary>
    public class TelnetRequest
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TelnetRequest" /> class.
        /// </summary>
        public TelnetRequest()
        {
        }

        /// <summary>
        /// Gets the Telnet command to be executed.
        /// </summary>
        public TelnetCommand Command { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool Authenticated { get; internal set; }

        /// <summary>
        /// Clears the <see cref="TelnetRequest"/> object.
        /// </summary>
        public void Clear()
        {
            Command.Clear();
            Authenticated = false;
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