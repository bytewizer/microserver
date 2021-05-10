using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents configuration options of server specific features.
    /// </summary>
    public class HttpServerOptions : ServerOptions
    {
        /// <summary>
        /// Provides access to request limit options.
        /// </summary>
        public HttpServerLimits Limits { get; } = new HttpServerLimits();

        /// <summary>
        /// Specifies the name the server represents.
        /// </summary>
        public string Name { get; set; } = "Microserver";
    }
}
