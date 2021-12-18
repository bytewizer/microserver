//using System;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using Bytewizer.TinyCLR.Logging;
//using Bytewizer.TinyCLR.Sockets.Channel;

//namespace Bytewizer.TinyCLR.Ftp.Channel
//{
//    /// <summary>
//    /// Active FTP data channel.
//    /// </summary>
//    internal sealed class PassiveDataChannel
//    {
//        private readonly ILogger _logger;
//        private readonly Thread _thread;
//        private readonly Socket _listenerSocket;

//        /// <summary>
//        /// Gets the listener IP end-point.
//        /// </summary>
//        public IPEndPoint EndPoint { get; private set; }

//        /// <summary>
//        /// Gets a value indicating whether the underlying Socket for a <see cref="ActiveDataChannel"/> is connected to a remote host.
//        /// </summary>
//        public bool Connected { get; internal set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ActiveDataChannel"/> class
//        /// with the provided end-point and logger.
//        /// </summary>
//        /// <param name="endPoint">End-point to connect to</param>
//        /// <param name="logger">Current logger instance</param>
//        public PassiveDataChannel(EndPoint endPoint, ILogger logger)
//        {
//            if (endPoint == null)
//            {
//                throw new ArgumentNullException(nameof(endPoint));
//            }

//            _listenerSocket =
//                new Socket(
//                    AddressFamily.InterNetwork,
//                    SocketType.Stream,
//                    ProtocolType.Tcp);

//            _listenerSocket.Bind(endPoint);

//            EndPoint = (IPEndPoint)_listenerSocket.LocalEndPoint;

//            _listenerSocket.Listen(1);

//            _thread = new Thread(() =>
//            {
//                if (_listenerSocket != null)
//                {
//                    AcceptConnection();
//                }
//            });

//            _thread.Start();
//        }

//        /// <summary>
//        /// Accepted connection listening thread
//        /// </summary>
//        private void AcceptConnection()
//        {
//        }


//        /// <summary>
//        /// Returns the <see cref="NetworkStream"/> used to send and receive data.
//        /// </summary>
//        /// <returns>The underlying <see cref="NetworkStream"/>.</returns>
//        public NetworkStream GetStream()
//        {
//            if (Connected == false)
//            {
//                throw new InvalidOperationException();
//            }

//            Stream stream;
//            if (_logger.IsEnabled(LogLevel.Trace))
//            {
//                stream = new LoggerStream(_logger,
//                    new NetworkStream(_listenerSocket, true)
//                    );
//            }
//            else
//            {
//                stream = new NetworkStream(_listenerSocket, true);
//            }

//            return stream as NetworkStream;
//        }
//    }
//}
