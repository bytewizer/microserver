using System.Collections;

namespace Bytewizer.TinyCLR.Telnet.Features
{
    /// <summary>
    /// A feature interface for this session. Use <see cref="TelnetContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public class EndpointFeature 
    {
        /// <summary>
        /// Gets or sets command endpoint for the current request.
        /// </summary>
        public Hashtable Endpoints { get; set; }

        /// <summary>
        /// Gets or sets command for the current request.
        /// </summary>
        public Hashtable Commands { get; set; }
    }
}