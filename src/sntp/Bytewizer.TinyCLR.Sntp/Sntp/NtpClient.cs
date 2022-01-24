using System;
using System.Net;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Represents UDP socket used to communicate with SNTP server.
    /// </summary>
    public class SntpClient : IDisposable
    {
        readonly Socket socket;

        /// <summary>
        /// Gets or sets the timeout for SNTP queries.
        /// </summary>
        /// <value>
        /// Timeout for SNTP queries. Default is one second.
        /// </value>
        public TimeSpan Timeout
        {
            get { return TimeSpan.FromMilliseconds(socket.ReceiveTimeout); }
            set
            {
                if (value < TimeSpan.FromMilliseconds(1))
                {
                    throw new ArgumentOutOfRangeException();
                }
                socket.ReceiveTimeout = (int)(value.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Creates new <see cref="SntpClient" /> from server endpoint.
        /// </summary>
        /// <param name="endpoint">Endpoint of the remote SNTP server.</param>
        public SntpClient(IPEndPoint endpoint)
            : this(endpoint, TimeSpan.FromSeconds(1)) { }

        /// <summary>
        /// Creates new <see cref="SntpClient" /> from server endpoint.
        /// </summary>
        /// <param name="endpoint">Endpoint of the remote SNTP server.</param>
        /// <param name="timeout">Timeout for SNTP queries.</param>
        public SntpClient(IPEndPoint endpoint, TimeSpan timeout)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Timeout = timeout;

            try
            {
                socket.Connect(endpoint);
            }
            catch
            {
                socket.Close();
                throw;
            }
        }

        /// <summary>
        /// Creates new <see cref="SntpClient" /> to the specified port on the specified host.
        /// </summary>
        /// <param name="hostname">The DNS name of the remote host to which you intend to connect.</param>
        /// <param name="port">The port number of the remote host to which you intend to connect.</param>
        public SntpClient(string hostname, int port)
        {
            if (string.IsNullOrEmpty(hostname))
            {
                throw new ArgumentNullException(nameof(hostname));
            }

            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(nameof(port));
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var hostEntry = Dns.GetHostEntry(hostname);

            if ((hostEntry != null) && (hostEntry.AddressList.Length > 0))
            {
                var i = 0;
                IPAddress remoteIpAddress;

                while (hostEntry.AddressList[i] == null) i++;
                {
                    remoteIpAddress = hostEntry.AddressList[i];
                }

                socket.Connect(new IPEndPoint(remoteIpAddress, port));
            }
            else
            {
                socket.Close();
                throw new Exception("Server not found."); ;
            }
        }


        /// <summary>
        /// Creates new <see cref="SntpClient" /> from server's IP address and optional port.
        /// </summary>
        /// <param name="address">IP address of remote SNTP server</param>
        /// <param name="port">Port of remote SNTP server. Default is 123 (standard NTP port).</param>
        public SntpClient(IPAddress address, int port = 123)
            : this(new IPEndPoint(address, port)) { }

        /// <summary>
        /// Creates new <see cref="SntpClient" /> from server's IP address and optional port.
        /// </summary>
        /// <param name="address">IP address of remote SNTP server</param>
        /// <param name="timeout">Timeout for SNTP queries.</param>
        /// <param name="port">Port of remote SNTP server. Default is 123 (standard NTP port).</param>
        public SntpClient(IPAddress address, TimeSpan timeout, int port = 123)
            : this(new IPEndPoint(address, port), timeout) { }

        /// <summary>
        /// Releases all resources held by <see cref="SntpClient" />.
        /// </summary>
        public void Dispose()
        {
            socket.Close();
        }

        /// <summary>
        /// Queries the SNTP server and returns correction offset.
        /// </summary>
        public TimeSpan GetCorrectionOffset() { return Query().CorrectionOffset; }

        /// <summary>
        /// Queries the SNTP server with configurable <see cref="NtpPacket" /> request.
        /// </summary>
        /// <param name="request">SNTP request packet to use when querying the network time server.</param>
        /// <returns>SNTP reply packet returned by the server.</returns>
        public NtpPacket Query(NtpPacket request)
        {
            request.ValidateRequest();
            
            socket.Send(request.Bytes);
            
            var response = new byte[160];
            int received = socket.Receive(response);
            
            var truncated = new byte[received];
            Array.Copy(response, truncated, received);
            
            NtpPacket reply = new NtpPacket(truncated) { DestinationTimestamp = DateTime.UtcNow };
            reply.ValidateReply(request);
            
            return reply;
        }

        /// <summary>
        /// Queries the SNTP server with default options.
        /// </summary>
        public NtpPacket Query()
        { 
            return Query(new NtpPacket());
        }
    }
}
