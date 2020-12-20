using System;
using System.Net;

using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents configuration options of server specific features.
    /// </summary>
    public class ServerOptions
    {
        internal IApplicationBuilder Pipeline { get; set; } = new ApplicationBuilder();

        /// <summary>
        /// Configuration options of socket specific features.
        /// </summary>
        public SocketListenerOptions Listener { get; private set; } = new SocketListenerOptions();

        /// <summary>
        /// Register a <see cref="IMiddleware"/> filter to the pipeline. Filters are executed in the order they are added.
        /// </summary>
        /// <param name="filter">The <see cref="Middleware"/> to include in the pipeline.</param>
        public void Register(IMiddleware filter)
        {
            Pipeline.Register(filter);
        }

        /// <summary>
        /// Bind to all ip address and port 80.
        /// </summary>
        public void Listen()
        {
            Listen(IPAddress.Any, 80);
        }

        /// <summary>
        /// Bind to the given port.
        /// </summary>
        /// <param name="port">The port for receiving data.</param>
        public void Listen(int port)
        {
            Listen(IPAddress.Any, port, _ => { });
        }

        /// <summary>
        /// Bind to given ip endpoint.
        /// </summary>
        /// <param name="endPoint">The endpoint to bind on.</param>
        public void Listen(IPEndPoint endPoint)
        {
            Listen(endPoint, _ => { });
        }

        /// <summary>
        /// Bind to the given ip address and port.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data.</param>
        public void Listen(IPAddress address, int port)
        {
            Listen(address, port, _ => { });
        }

        /// <summary>
        /// Bind to the given ip address, port and configuration.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data.</param>
        /// <param name="configure">The <see cref="SocketListener"/> configuration options features.</param>
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

        /// <summary>
        /// Bind to the given end point and configuration.
        /// </summary>
        /// <param name="endPoint">The endpoint for receiving data.</param>
        /// <param name="configure">The <see cref="SocketListener"/> configuration options features.</param>
        public void Listen(IPEndPoint endPoint, SocketListenerOptionsDelegate configure)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            var listenOptions = new SocketListenerOptions(endPoint);
            
            configure(listenOptions);

            Listener = listenOptions;
        }
    }
}