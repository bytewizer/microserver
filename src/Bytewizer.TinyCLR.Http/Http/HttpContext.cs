using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Http.Features;
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
            Features = new FeatureCollection();
            Request = new HttpRequest();
            Response = new HttpResponse();
            Channel = new SocketChannel();
        }

        /// <summary>
        /// Gets the collection of HTTP features provided by the server 
        /// and middleware available on this request.
        /// </summary>
        public IFeatureCollection Features { get; }

        /// <summary>
        /// Gets the <see cref="HttpRequest"/> object for this request.
        /// </summary>
        public HttpRequest Request { get; private set; }

        /// <summary>
        /// Gets the <see cref="HttpResponse"/> object for this request.
        /// </summary>
        public HttpResponse Response { get; private set; }

        /// <summary>
        /// Gets or sets the object used to manage user session data for this request.
        /// </summary>
        public SocketChannel Channel { get; set; }

        /// <summary>
        /// Gets information about the underlying connection for this request.
        /// </summary>
        public ConnectionInfo Connection => Channel.Connection;

        /// <summary>
        /// Aborts the connection underlying this request.
        /// </summary>
        public void Abort()
        {
            Channel?.Clear();
        }

        /// <summary>
        /// Closes and clears the connected socket channel and response body.
        /// </summary>
        public void Clear()
        {
            Response.Body?.Dispose();        
            Channel?.Clear();
        }
    }
}