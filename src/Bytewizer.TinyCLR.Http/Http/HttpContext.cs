using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Encapsulates all HTTP-specific information about an individual HTTP request.
    /// </summary>
    public class HttpContext : IContext
    {

        /// <summary>
        /// Initializes an instance of the <see cref="HttpContext" /> class.
        /// </summary>
        public HttpContext() 
        {
            Request = new HttpRequest();
            Response = new HttpResponse();
            Items = new ItemsDictionary();
        }

        /// <summary>
        /// Gets information about the underlying connection for this request.
        /// </summary>
        public ConnectionInfo Connection => Channel.Connection;

        /// <summary>
        /// Gets the <see cref="HttpRequest"/> object for this request.
        /// </summary>
        public HttpRequest Request { get; private set; }

        /// <summary>
        /// Gets the <see cref="HttpResponse"/> object for this request.
        /// </summary>
        public HttpResponse Response { get; private set; }

        /// <summary>
        /// Gets or sets a key/value collection that can be used to share data within the scope of this request.
        /// </summary>
        public ItemsDictionary Items { get; set; } 

        /// <summary>
        /// Gets or sets the object used to manage user session data for this request.
        /// </summary>
        public SocketChannel Channel { get; set; } = new SocketChannel();

        /// <summary>
        /// Gets or sets security information for the current HTTP request.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Aborts the connection underlying this request.
        /// </summary>
        public void Abort()
        {
            Channel?.Clear();
        }
    }
}