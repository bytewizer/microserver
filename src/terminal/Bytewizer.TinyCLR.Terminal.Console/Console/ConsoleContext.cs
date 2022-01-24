using Bytewizer.TinyCLR.Features;

using Bytewizer.TinyCLR.Terminal.Channel;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Encapsulates all Terminal spcific information about an individual request.
    /// </summary>
    public class ConsoleContext : TerminalContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="ConsoleContext" /> class.
        /// </summary>
        public ConsoleContext() 
            : base()
        {
            Channel = new ConsoleChannel();
        }

        /// <summary>
        /// Gets or sets the object used to manage user session data for this request.
        /// </summary>
        public ConsoleChannel Channel { get; set; }

        /// <summary>
        /// Clears connected context.
        /// </summary>
        public override void Clear()
        {
            ((FeatureCollection)Features).Clear();
            Options.Clear();
            Request.Clear();
            Response.Clear();
            Channel.Clear();
            Active = true;
        }
    }
}