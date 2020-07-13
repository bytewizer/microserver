using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.Sockets
{
    public class SocketListenerOptions
    {
        public IPEndPoint IPEndPoint => EndPoint as IPEndPoint;

        public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12;

        // Socket options
        public SocketType SocketType { get; set; } = SocketType.Stream;

        public ProtocolType ProtocolType { get; set; } = ProtocolType.Tcp;

        public bool ReuseAddress { get; set; } = true;

        public bool KeepAlive { get; set; } = false;

        public bool Broadcast { get; set; } = false;

        // Timeout
        public int SendTimeout { get; set; } = -1; // default value

        public int ReceiveTimeout { get; set; } = -1; // default value

        // Socket threading
        public int MinThreads { get; set; } = -1;  //3;  TODO: Threading add about 30ms to the response time.

        public int MaxThreads { get; set; } = -1;  //10;

        public TimeSpan HandshakeTimeout { get; set; } = TimeSpan.FromSeconds(3);

        public int MaxCountOfPendingConnections { get; set; } = 255;

        public int DefaultKeepAliveMilliseconds { get; set; } = 60000;

        public override string ToString()
        {
            return $"{Scheme}://{IPEndPoint}";
        }

        // Interal options
        internal IPEndPoint EndPoint { get; set; }

        internal X509Certificate Certificate { get; set; }
        
        internal bool IsTls { get; set; }

        internal SocketListenerOptions()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, 80);
        }

        internal SocketListenerOptions(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        internal string Scheme
        {
            get { return IsTls ? "https" : "http"; }
        }
    }
}