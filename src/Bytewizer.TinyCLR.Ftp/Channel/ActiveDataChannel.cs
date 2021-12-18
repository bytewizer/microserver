using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Ftp.Channel
{
    /// <summary>
    /// Active FTP data channel.
    /// </summary>
    internal sealed class ActiveDataChannel 
    {
        private readonly ILogger _logger;
        private readonly Socket _clientSocket;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDataChannel"/> class
        /// with the provided end-point and logger.
        /// </summary>
        /// <param name="endPoint">End-point to connect to</param>
        /// <param name="logger">Current logger instance</param>
        public ActiveDataChannel(EndPoint endPoint, ILogger logger)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            _logger = logger;
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

            _clientSocket.Connect(endPoint);
            Connected = true;
        }

        /// <summary>
        /// Gets a value indicating whether the underlying Socket for a <see cref="ActiveDataChannel"/> is connected to a remote host.
        /// </summary>
        public bool Connected { get; internal set; }

        /// <summary>
        /// Returns the <see cref="NetworkStream"/> used to send and receive data.
        /// </summary>
        /// <returns>The underlying <see cref="NetworkStream"/>.</returns>
        public NetworkStream GetStream()
        {
            if (Connected == false)
            {
                throw new InvalidOperationException();
            }

            Stream stream;
            if (_logger.IsEnabled(LogLevel.Trace))
            {
               stream = new LoggerStream(_logger,
                   new NetworkStream(_clientSocket, true)
                   );
            }
            else
            {
                stream = new NetworkStream(_clientSocket, true);
            }

            return stream as NetworkStream;
        }
    }
}
