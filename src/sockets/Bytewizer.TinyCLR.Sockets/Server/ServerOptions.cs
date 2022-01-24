using System;
using System.Net;

using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Pipeline.Builder;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents configuration options of server specific features.
    /// </summary>
    public class ServerOptions : IServerOptions
    {
        private readonly IApplicationBuilder _applicationBuilder = new ApplicationBuilder();
        
        /// <summary>
        /// Configuration options of socket specific features.
        /// </summary>
        internal SocketListenerOptions Listener { get; private set; } = new SocketListenerOptions();

        /// <inheritdoc/>
        public ServerLimits Limits { get; } = new ServerLimits();

        /// <inheritdoc/>
        public IApplication Application { get; private set; }

        /// <inheritdoc/>
        public void Pipeline(ApplicationDelegate configure)
        {
            configure(_applicationBuilder);
            Application = _applicationBuilder.Build();
        }

        /// <inheritdoc/>
        public void Listen()
        {
            Listen(IPAddress.Any, 0);
        }

        /// <inheritdoc/>
        public void Listen(int port)
        {
            Listen(IPAddress.Any, port, _ => { });
        }

        /// <inheritdoc/>
        public void Listen(IPEndPoint endPoint)
        {
            Listen(endPoint, _ => { });
        }

        /// <inheritdoc/>
        public void Listen(IPAddress address, int port)
        {
            Listen(address, port, _ => { });
        }

        /// <inheritdoc/>
        public void Listen(int port, SocketListenerOptionsDelegate configure)
        {
            Listen(IPAddress.Any, port, configure);
        }

        /// <inheritdoc/>
        public void Listen(IPAddress address, int port, SocketListenerOptionsDelegate configure)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort)
            {
                throw new ArgumentOutOfRangeException(nameof(port), $"Must be less then { IPEndPoint.MaxPort } and greater then { IPEndPoint.MinPort }.");
            }

            Listen(new IPEndPoint(address, port), configure);
        }

        /// <inheritdoc/>
        public void Listen(IPEndPoint endPoint, SocketListenerOptionsDelegate configure)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            Listener.EndPoint = endPoint;
            configure(Listener);
        }
    }
}