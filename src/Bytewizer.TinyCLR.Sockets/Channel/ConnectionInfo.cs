using System;
using System.Net;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets.Channel
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
            Id = DateTime.Now.Ticks.ToString(); //TODO: Switch to Guid - GHI Github issue #476,
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
            Id = DateTime.Now.Ticks.ToString(); //TODO: Switch to Guid - GHI Github issue #476,
            LocalEndpoint = socket.LocalEndPoint;
            RemoteEndpoint = endpoint;
        }


        //    internal static ConnectionInfo Set(Socket socket)
        //{
        //    return new ConnectionInfo()
        //    {
        //        Id = DateTime.Now.Ticks.ToString(), //TODO: Switch to Guid - GHI Github issue #476,
        //        LocalEndpoint = socket.LocalEndPoint,
        //        RemoteEndpoint = socket.RemoteEndPoint,
        //    };
        //}
        //internal static ConnectionInfo Set(Socket socket, EndPoint endpoint)
        //{
        //    return new ConnectionInfo()
        //    {
        //        Id = DateTime.Now.Ticks.ToString(), //TODO: Switch to Guid - GHI Github issue #476,
        //        LocalEndpoint = socket.LocalEndPoint,
        //        RemoteEndpoint = endpoint,
        //    };
        //}
    }
}