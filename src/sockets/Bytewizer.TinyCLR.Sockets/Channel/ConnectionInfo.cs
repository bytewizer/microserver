using System;
using System.Net;
using System.Net.Sockets;

#if NanoCLR
namespace Bytewizer.NanoCLR.Sockets.Channel
#else
namespace Bytewizer.TinyCLR.Sockets.Channel
#endif
{
    /// <summary>
    /// Represents a socket connection between two end points.
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>
        /// Get the identity of this channel.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Gets endpoint of the local end point.
        /// </summary>
        public EndPoint LocalEndpoint { get; internal set; }

        /// <summary>
        /// Gets address of the local end point.
        /// </summary>
        public IPAddress LocalIpAddress 
        {
            get { return ((IPEndPoint)LocalEndpoint).Address; }
        }

        /// <summary>
        /// Gets port of the local end point.
        /// </summary>
        public int LocalPort
        {
            get { return ((IPEndPoint)LocalEndpoint).Port; }
        }

        /// <summary>
        /// Gets endpoint of the connected end point.
        /// </summary>
        public EndPoint RemoteEndpoint { get; internal set; }

        /// <summary>
        /// Gets address of the connected end point.
        /// </summary>
        public IPAddress RemoteIpAddress
        {
            get { return ((IPEndPoint)RemoteEndpoint).Address; }
        }

        /// <summary>
        /// Gets port of the connected end point.
        /// </summary>
        public int RemotePort
        {
            get { return ((IPEndPoint)RemoteEndpoint).Port; }
        }

        /// <summary>
        /// Assign a connection information to this channel.
        /// </summary>
        /// <param name="socket">The connected socket for channel.</param>
        internal void Assign(Socket socket)
        {
            Id = DateTime.UtcNow.Ticks.ToString(); //TODO: Switch to Guid - GHI Github issue #476,
            LocalEndpoint = socket.LocalEndPoint;
            RemoteEndpoint = socket.RemoteEndPoint;
        }

        /// <summary>
        /// Assign a connection information to this channel.
        /// </summary>
        /// <param name="socket">The connected socket for channel. </param>
        /// <param name="endpoint">The remote endpoint of the connected socket. </param>
        internal void Assign(Socket socket, EndPoint endpoint)
        {
            Id = DateTime.UtcNow.Ticks.ToString(); //TODO: Switch to Guid - GHI Github issue #476,
            LocalEndpoint = socket.LocalEndPoint;
            RemoteEndpoint = endpoint;
        }
    }
}