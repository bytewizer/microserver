using System.Net;

using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Pipeline.Builder;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// An interface for <see cref="ServerOptions"/>.
    /// </summary>
    public interface IServerOptions
    {
        /// <summary>
        /// Provides access to message limit options.
        /// </summary>
        ServerLimits Limits { get; }

        /// <summary>
        /// An application pipeline for registered middleware. 
        /// </summary>
        IApplication Application { get; }

        /// <summary>
        /// Bind to all ip address and let the OS assign a port.
        /// </summary>
        /// <remarks>
        /// You can use <see cref="SocketListener.ActivePort" /> to give you the assigned port.
        /// </remarks>
        void Listen();

        /// <summary>
        /// Bind to the given port.
        /// </summary>
        /// <param name="port">The port for receiving data. You can use port <c>0</c> to let the OS assign a port.</param>
        /// <remarks>
        /// You can use <see cref="SocketListener.ActivePort" /> to give you the assigned port.
        /// </remarks>
        void Listen(int port);

        /// <summary>
        /// Bind to the given ip address, port and configuration.
        /// </summary>
        /// <param name="port">The port for receiving data. You can use port <c>0</c> to let the OS assign a port.</param>
        /// <param name="configure">The <see cref="SocketListener"/> configuration options features.</param>
        /// <remarks>
        /// You can use <see cref="SocketListener.ActivePort" /> to give you the assigned port.
        /// </remarks>
        void Listen(int port, SocketListenerOptionsDelegate configure);

        /// <summary>
        /// Bind to the given ip address and port.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data. You can use port <c>0</c> to let the OS assign a port.</param>
        /// <remarks>
        /// You can use <see cref="SocketListener.ActivePort" /> to give you the assigned port.
        /// </remarks>
        void Listen(IPAddress address, int port);

        /// <summary>
        /// Bind to the given ip address, port and configuration.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data. You can use port <c>0</c> to let the OS assign a port.</param>
        /// <param name="configure">The <see cref="SocketListener"/> configuration options features.</param>
        /// <remarks>
        /// You can use <see cref="SocketListener.ActivePort" /> to give you the assigned port.
        /// </remarks>
        void Listen(IPAddress address, int port, SocketListenerOptionsDelegate configure);

        /// <summary>
        /// Bind to given ip endpoint.
        /// </summary>
        /// <param name="endPoint">The endpoint to bind on.</param>
        void Listen(IPEndPoint endPoint);

        /// <summary>
        /// Bind to the given end point and configuration.
        /// </summary>
        /// <param name="endPoint">The endpoint for receiving data.</param>
        /// <param name="configure">The <see cref="SocketListener"/> configuration options features.</param>
        void Listen(IPEndPoint endPoint, SocketListenerOptionsDelegate configure);

        /// <summary>
        /// Configures <see cref="IApplication"/> pipeline options. <see cref="Middleware"/> are executed in the order they are added.
        /// </summary>
        /// <param name="configure">The delegate for configuring the <see cref="IApplicationBuilder"/> that will be used to construct the <see cref="IApplication"/>.</param>
        void Pipeline(ApplicationDelegate configure);
    }
}