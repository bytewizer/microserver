using Bytewizer.TinyCLR.Features;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Encapsulates all Terminal spcific information about an individual request.
    /// </summary>
    public class TerminalContext : ITerminalContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TerminalContext" /> class.
        /// </summary>
        public TerminalContext()
        {
            Features = new FeatureCollection();
            Options = new TerminalOptions();
            Request = new TerminalRequest();
            Response = new TerminalResponse();
        }

        /// <summary>
        /// Gets the collection of Terminal features provided by the server 
        /// and middleware available on this request.
        /// </summary>
        public IFeatureCollection Features { get; }

        /// <summary>
        /// Gets the <see cref="Options"/> object for this request.
        /// </summary>
        public TerminalOptions Options { get; private set; }

        /// <summary>
        /// Gets the <see cref="TerminalRequest"/> object for this request.
        /// </summary>
        public TerminalRequest Request { get; private set; }

        /// <summary>
        /// Gets the <see cref="TerminalResponse"/> object for this request.
        /// </summary>
        public TerminalResponse Response { get; private set; }

        /// <summary>
        /// Gets or sets the incoming commands for this request.
        /// </summary>
        public byte[] OptionCommands { get; set; }

        /// <summary>
        /// Get the session state for this request.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Closes the connected socket channel and clears context.
        /// </summary>
        public virtual void Clear()
        {
            ((FeatureCollection)Features).Clear();
            Options.Clear();
            Request.Clear();
            Response.Clear();
            Active = true;
        }
    }
}