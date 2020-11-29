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
        
        internal static ConnectionInfo Set(Socket channel)
        {
            return new ConnectionInfo()
            {
                Id = DateTime.Now.Ticks.ToString(), //TODO: GHI Github issue #476 / Id = _context.Channel.Id.ToString(),
                LocalIpAddress = ((IPEndPoint)channel.LocalEndPoint).Address,
                LocalPort = ((IPEndPoint)channel.LocalEndPoint).Port,
                RemoteIpAddress = ((IPEndPoint)channel.RemoteEndPoint).Address,
                RemotePort = ((IPEndPoint)channel.RemoteEndPoint).Port
            };
        }
    }
}