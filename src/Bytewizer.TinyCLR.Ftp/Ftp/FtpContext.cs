using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Encapsulates all FTP-specific information about an individual FTP request.
    /// </summary>
    public class FtpContext : IContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="FtpContext" /> class.
        /// </summary>
        public FtpContext() 
        {
            Command = new FtpCommand();
            Request = new FtpRequest();
            Response = new FtpResponse();
            Channel = new SocketChannel();
        }

        /// <summary>
        /// Gets the FTP command to be executed.
        /// </summary>
        public FtpCommand Command { get; private set; }

        /// <summary>
        /// Gets the <see cref="FtpRequest"/> object for this request.
        /// </summary>
        public FtpRequest Request { get; private set; }

        /// <summary>
        /// Gets the <see cref="FtpResponse"/> object for this request.
        /// </summary>
        public FtpResponse Response { get; private set; }

        /// <summary>
        /// Gets or sets the object used to manage user session data for this request.
        /// </summary>
        public SocketChannel Channel { get; set; }

        /// <summary>
        /// Gets information about the underlying connection for this request.
        /// </summary>
        public ConnectionInfo Connection => Channel?.Connection;

        /// <summary>
        /// Aborts the connection underlying this request.
        /// </summary>
        public void Abort()
        {
            Channel?.Socket?.Close();
        }

        /// <summary>
        /// Closes the connected socket channel and clears context.
        /// </summary>
        public void Clear()
        {
            Command.Clear();
            Request.Clear();
            Response.Clear();      
            Channel.Clear();
        }
    }
}