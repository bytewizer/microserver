namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Encapsulates all WebSocket-specific information about an individual caller.
    /// </summary>
    public class HubCallerContext 
    {
        private readonly HttpContext _context;

        /// <summary>
        /// Initializes an instance of the <see cref="HubCallerContext" /> class.
        /// </summary>
        public HubCallerContext()
        {
        
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HubCallerContext" /> class.
        /// </summary>
        public HubCallerContext(HttpContext context) 
        {
            _context = context;
        }

        /// <summary>
        /// Gets the connection id of the calling client.
        /// </summary>
        public string ConnectionId => _context?.Connection.Id;

        /// <summary>
        /// Gets the <see cref="HttpContext"/> for the current handshake request.
        /// </summary>
        public HttpContext GetHttpContext()
        {
            return _context;
        }

        /// <summary>
        /// Aborts the connection underlying this request.
        /// </summary>
        public void Abort()
        {
            _context?.Channel.Socket.Close();
        }
    }
}