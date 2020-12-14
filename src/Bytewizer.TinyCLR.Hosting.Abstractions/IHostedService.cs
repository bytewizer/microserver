using System.Threading;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Defines methods for objects that are managed by the host.
    /// </summary>
    public interface IHostedService
    {
        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        void Start();

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        void Stop();
    }
}
