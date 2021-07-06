using System;
using System.Net;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UdpClient"/> class.
    /// </summary>
    public class UdpClient : IDisposable
    {
        private bool _active;
        private Socket _clientSocket;
        private NetworkStream _dataStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        public UdpClient()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize client with a given server IP address and port number
        /// </summary>
        /// <param name="address">The ip address of the host to which you intend to connect.</param>
        /// <param name="port">The port number to which you intend to connect.</param>
        public UdpClient(string address, int port)
            : this(new IPEndPoint(IPAddress.Parse(address), port))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class and binds it to the specified local endpoint.
        /// </summary>
        /// <param name="localEP">The <see cref="IPEndPoint"/> to which you bind the TCP <see cref="Socket"/>.</param>
        public UdpClient(IPEndPoint localEP)
        {
            if (localEP == null)
            {
                throw new ArgumentNullException(nameof(localEP));
            }

            if (localEP.Port < IPEndPoint.MinPort || localEP.Port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(nameof(localEP.Port));
            }

            Initialize();
            Client.Bind(localEP);
        }

        // used by TcpListener.AcceptTcpClient()
        internal UdpClient(Socket acceptedSocket)
        {
            Client = acceptedSocket;
            _active = true;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a connection has been made.
        /// </summary>
        public bool Active
        {
            get { return _active; }
            protected set { _active = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying Socket for a <see cref="TcpClient"/> is connected to a remote host.
        /// </summary>
        public bool Connected { get; internal set; }

        /// <summary>
        /// Gets the amount of data that has been received from the network and is available to be read.
        /// </summary>
        public int Available { get { return Client.Available; } }

        /// <summary>
        /// Gets or sets the underlying <see cref="Socket"/>.
        /// </summary>
        public Socket Client
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        /// <summary>
        /// Gets or sets the size of the receive buffer.
        /// </summary>
        public int ReceiveBufferSize
        {
            get { return NumericOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer); }
            set { Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value); }
        }

        /// <summary>
        /// Gets or sets the size of the send buffer.
        /// </summary>
        public int SendBufferSize
        {
            get { return NumericOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer); }
            set { Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value); }
        }

        /// <summary>
        /// Gets or sets information about the linger state of the associated <see cref="Socket"/>.
        /// </summary>
        public int LingerState
        {
            get { return (int)Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger); }
            set { Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value); }
        }

        /// <summary>
        /// Gets or sets a value that disables a delay when send or receive buffers are not full.
        /// </summary>
        public bool NoDelay
        {
            get { return NumericOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay) != 0 ? true : false; }
            set { Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, value ? 1 : 0); }
        }

        /// <summary>
        /// Gets or sets the amount of time a <see cref="TcpClient"/> will wait to receive data once a read operation is initiated.
        /// </summary>
        public int ReceiveTimeout
        {
            get { return NumericOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout); }
            set { Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value); }
        }

        /// <summary>
        /// Gets or sets the amount of time a <see cref="TcpClient"/> will wait for a send operation to complete successfully.
        /// </summary>
        public int SendTimeout
        {
            get { return NumericOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout); }
            set { Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value); }
        }

        /// <summary>
        /// Connects the client to the specified port on the specified host.
        /// </summary>
        /// <param name="hostname">The DNS name of the remote host to which you intend to connect.</param>
        /// <param name="port">The port number of the remote host to which you intend to connect.</param>
        public void Connect(string hostname, int port)
        {
            if (string.IsNullOrEmpty(hostname))
            {
                throw new ArgumentNullException(nameof(hostname));
            }

            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(nameof(port));
            }

            if (_active)
            {
                throw new SocketException(SocketError.IsConnected);
            }

            var hostEntry = Dns.GetHostEntry(hostname);

            if ((hostEntry != null) && (hostEntry.AddressList.Length > 0))
            {
                var i = 0;
                IPAddress remoteIpAddress;

                while (hostEntry.AddressList[i] == null) i++;
                {
                    remoteIpAddress = hostEntry.AddressList[i];
                }

                Connect(new IPEndPoint(remoteIpAddress, port));
            }
            else
            {
                throw new Exception("Server not found."); ;
            }
        }

        /// <summary>
        /// Connects the client to a remote TCP host using the specified remote network endpoint.
        /// </summary>
        /// <param name="remoteEP">The <see cref="IPEndPoint"/> to which you intend to connect.</param>
        public void Connect(IPEndPoint remoteEP)
        {
            if (remoteEP == null)
            {
                throw new ArgumentNullException(nameof(remoteEP));
            }

            if (remoteEP.Port < IPEndPoint.MinPort || remoteEP.Port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(nameof(remoteEP.Port));
            }

            Client.Connect(remoteEP);

            Connected = true;
        }

        /// <summary>
        /// Returns a UDP datagram that was sent by a remote host.
        /// </summary>
        /// <param name="remoteEP">An <see cref="IPEndPoint"/> that represents the remote host from which the data was sent.</param>
        public byte[] Receive(ref IPEndPoint remoteEP)
        {
            byte[] buffer = new byte[65536]; // Max. size
            EndPoint endPoint = remoteEP;

            int dataRead = Client.ReceiveFrom(buffer, ref endPoint);
            if (dataRead < buffer.Length)
            {
                remoteEP = (IPEndPoint)endPoint;
            }

            return buffer;
        }

        /// <summary>
        /// Disposes this <see cref="UdpClient"/> instance and requests that the underlying TCP connection be closed.
        /// </summary>
        public void Close()
        {
            Dispose();
            _active = false;
            Connected = false;
        }

        /// <summary>
        /// Returns the <see cref="NetworkStream"/> used to send and receive data.
        /// </summary>
        /// <returns>The underlying <see cref="NetworkStream"/>.</returns>
        public NetworkStream GetStream()
        {
            if (!Active)
            {
                throw new InvalidOperationException();
            }

            if (_dataStream == null)
            {
                _dataStream = new NetworkStream(Client, true);
            }

            return _dataStream; ;
        }

        private void Initialize()
        {
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Connected = false;
            _active = false;
        }

        private int NumericOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            return (int)Client.GetSocketOption(optionLevel, optionName);
        }

        /// <summary>
        /// Frees resources used by the <see cref="UdpClient"/> class.
        /// </summary>
        ~UdpClient()
        {
            {
                Dispose(false);
            }
        }

        /// <summary>
        /// Releases the managed and unmanaged resources used by the <see cref="UdpClient"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="UdpClient"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">Set to <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable dataStream = _dataStream;
                if (dataStream != null)
                {
                    dataStream.Dispose();
                }
                else
                {
                    Socket chkClientSocket = Client;
                    if (chkClientSocket != null)
                    {
                        chkClientSocket.Close();
                        Client = null;
                    }
                }

                GC.SuppressFinalize(this);
            }
        }
    }
}