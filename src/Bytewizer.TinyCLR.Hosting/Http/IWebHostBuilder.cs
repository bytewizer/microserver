using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Hosting
{
    public interface IWebHostBuilder
    {
        /// <summary>
        /// Builds an <see cref="IServer"/> which hosts a web application.
        /// </summary>
        IServer Build();
    }
}
