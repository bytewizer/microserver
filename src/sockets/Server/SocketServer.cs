using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets
{
    public class SocketServer : SocketService
    {
        public SocketServer()
        {
        }

        public SocketServer(IPipelineBuilder pipeline)
            : base(pipeline)
        {
        }

        public SocketServer(int port)
            : base(port)
        {
        }

        public SocketServer(ServerOptionsDelegate configure)
            : base(configure)
        {
        }

        public SocketServer(IPAddress address, int port, IPipelineBuilder pipeline)
            : base(address, port, pipeline)
        {
        }
    }
}