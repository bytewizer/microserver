using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// Represents configuration options of socket specific features.
    /// </summary>
    public class SocketListenerOptions
    {
        /// <summary>
        /// Specifies a network <see cref="IPEndPoint"/> as an IP address and a port number.
        /// </summary>
        public IPEndPoint IPEndPoint => EndPoint;

        /// <summary>
        /// Specifies to secure the connection using Tls.
        /// </summary>
        public bool IsTls { get; internal set; }

        /// <summary>
        /// Specifies the possible versions of SslProtocols that the <see cref="SocketListener"/> class represents.
        /// </summary>
        public SslProtocols SslProtocols { get; internal set; } = SslProtocols.Tls12;

        /// <summary>
        /// Specifies the X.509 certificate that the <see cref="SocketListener"/> class represents.
        /// </summary>
        public X509Certificate Certificate { get; internal set; }

        /// <summary>
        /// Specifies the type of socket that the <see cref="SocketListener"/> class represents.
        /// </summary>
        public SocketType SocketType { get; set; } = SocketType.Stream;

        /// <summary>
        /// Specifies the protocols that the <see cref="SocketListener"/> class represents.
        /// </summary>
        public ProtocolType ProtocolType { get; set; } = ProtocolType.Tcp;

        /// <summary>
        /// Specifies the <see cref="SocketListener"/> to be bound to an address that is already in use.
        /// </summary>
        public bool ReuseAddress { get; set; } = true;

        /// <summary>
        /// Specifies the <see cref="SocketListener"/> to use keep-alives.
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>
        /// Permit sending broadcast messages on the <see cref="SocketListener"/>.
        /// </summary>
        public bool Broadcast { get; set; } = false;

        /// <summary>
        /// Disables the Nagle algorithm for send coalescing.
        /// </summary>
        public bool NoDelay { get; set; } = true;

        /// <summary>
        /// Specifies the <see cref="SocketListener"/> send time-out.
        /// </summary>
        public int SendTimeout { get; set; } = -1; // default value

        /// <summary>
        /// Specifies the <see cref="SocketListener"/> receive time-out.
        /// </summary>
        public int ReceiveTimeout { get; set; } = -1; // default value

        /// <summary>
        /// Specifies the <see cref="SocketListener"/> socket retry limit before failing.
        /// </summary>
        public int SocketRetry { get; set; } = 5;

        /// <summary>
        /// Specifies the scheduling priority of <see cref="SocketListener"/>.
        /// </summary>
        public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.AboveNormal;

        /// <summary>
        /// Specifies the minimum number of threads the <see cref="SocketListener"/> thread pool creates on demand,
        /// as new requests are made, before switching to an algorithm for managing thread creation and destruction.
        /// </summary>
        public int MinThreads { get; set; } = 5;

        /// <summary>
        /// Specifies the number of requests to the <see cref="SocketListener"/> thread pool that can be active
        /// concurrently. All requests above this number remain queued until thread pool threads become available.
        /// </summary>
        public int MaxThreads { get; set; } = 20;

        /// <summary>
        /// Specifies the maximum backlog of the <see cref="SocketListener"/> pending connections queue.
        /// </summary>
        public int MaxPendingConnections { get; set; } = 128;

        /// <summary>
        /// Specifies in micro seconds how long to block execution until the <see cref="UdpListener"/> poll will wait.
        /// </summary>
        public int PollTimeout { get; set; } = 2000000; // -1 ;

        /// <summary>
        /// Specifies the maximum number of  the  <see cref="SocketListener"/> open connections. 
        /// </summary>
        public int MaxConcurrentConnections { get; set; } = 10;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{Scheme}://{IPEndPoint}";
        }

        // Interal options
        internal IPEndPoint EndPoint { get; set; }
   
        public SocketListenerOptions()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public SocketListenerOptions(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        internal string Scheme
        {
            get { return IsTls ? "https" : "http"; }
        }
    }
}