using System;
using System.Net;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets
{
    public class ConnectionInfo
    {
        public string Id { get; internal set; }

        public IPAddress LocalIpAddress { get; internal set; }

        public int LocalPort { get; internal set; }

        public IPAddress RemoteIpAddress { get; internal set; }

        public int RemotePort { get; internal set; }
        
        public static ConnectionInfo Set(Socket channel)
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