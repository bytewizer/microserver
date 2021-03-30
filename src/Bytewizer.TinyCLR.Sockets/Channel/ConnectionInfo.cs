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
        /// Gets address of the local end point.
        /// </summary>
        public IPAddress LocalIpAddress { get; internal set; }

        /// <summary>
        /// Gets port of the local end point.
        /// </summary>
        public int LocalPort { get; internal set; }

        /// <summary>
        /// Gets address of the connected end point.
        /// </summary>
        public IPAddress RemoteIpAddress { get; internal set; }

        /// <summary>
        /// Gets port of the connected end point.
        /// </summary>
        public int RemotePort { get; internal set; }
        
        internal static ConnectionInfo Set(Socket socket)
        {
            return new ConnectionInfo()
            {
                Id = DateTime.Now.Ticks.ToString(), //TODO: Switch to Guid - GHI Github issue #476,
                LocalIpAddress = ((IPEndPoint)socket.LocalEndPoint).Address,
                LocalPort = ((IPEndPoint)socket.LocalEndPoint).Port,
                RemoteIpAddress = ((IPEndPoint)socket.RemoteEndPoint).Address,
                RemotePort = ((IPEndPoint)socket.RemoteEndPoint).Port
            };
        }
        internal static ConnectionInfo Set(Socket socket, EndPoint endpoint)
        {
            return new ConnectionInfo()
            {
                Id = DateTime.Now.Ticks.ToString(), //TODO: Switch to Guid - GHI Github issue #476,
                LocalIpAddress = ((IPEndPoint)socket.LocalEndPoint).Address,
                LocalPort = ((IPEndPoint)socket.LocalEndPoint).Port,
                RemoteIpAddress = ((IPEndPoint)endpoint).Address,
                RemotePort = ((IPEndPoint)endpoint).Port
            };
        }
    }
}