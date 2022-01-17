using System.Collections;

namespace Bytewizer.TinyCLR.Terminal.Features
{
    /// <summary>
    /// A feature interface for this session. Use <see cref="TerminalContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public class EndpointFeature 
    {
        /// <summary>
        /// Gets or sets command endpoint for the current request.
        /// </summary>
        public Hashtable Endpoints { get; set; } = new Hashtable();

        /// <summary>
        /// Gets or sets command list for the current request.
        /// </summary>
        public Hashtable AvailableCommands { get; set; } = new Hashtable();

        /// <summary>
        /// Gets or sets a flag to enable auto mapping assemblies provider.
        /// </summary>
        public bool AutoMapping { get; set; }
    }
}