using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Features;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Encapsulates all Telnet spcific information about an individual request.
    /// </summary>
    public class TelnetContext : IContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TelnetContext" /> class.
        /// </summary>
        public TelnetContext() 
        {
            Features = new FeatureCollection();
            Request = new TelnetRequest();
            Response = new TelnetResponse();
            Channel = new SocketChannel();
        }

        /// <summary>
        /// Gets the collection of Telnet features provided by the server 
        /// and middleware available on this request.
        /// </summary>
        public IFeatureCollection Features { get; }

        /// <summary>
        /// Gets the <see cref="TelnetRequest"/> object for this request.
        /// </summary>
        public TelnetRequest Request { get; private set; }

        /// <summary>
        /// Gets the <see cref="TelnetResponse"/> object for this request.
        /// </summary>
        public TelnetResponse Response { get; private set; }

        /// <summary>
        /// Gets or sets the object used to manage user session data for this request.
        /// </summary>
        public SocketChannel Channel { get; set; }

        /// <summary>
        /// Gets or sets the incoming commands for this request.
        /// </summary>
        public byte[] OptionCommands { get; set; }

        /// <summary>
        /// Gets information about the underlying connection for this request.
        /// </summary>
        public ConnectionInfo Connection => Channel?.Connection;

        /// <summary>
        /// Get the session state for this request.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Closes the connected socket channel and clears context.
        /// </summary>
        public void Clear()
        {
            ((FeatureCollection)Features).Clear();
            Request.Clear();
            Response.Clear();
            Channel.Clear();
            Active = true;
        }
    }
}