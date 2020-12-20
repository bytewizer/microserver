using System;

using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Represents a configured web host.
    /// </summary>
    public interface IWebHost : IDisposable
    {
        /// <summary>
        /// The <see cref="IFeatureCollection"/> exposed by the configured server.
        /// </summary>
        IFeatureCollection ServerFeatures { get; }

        /// <summary>
        /// The <see cref="IServiceProvider"/> for the host.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Start the server accepting incoming requests.
        /// </summary>
        bool Start();

        /// <summary>
        /// Stop processing requests and gracefully shut down the server if possible.
        /// </summary>
        bool Stop();
    }
}
