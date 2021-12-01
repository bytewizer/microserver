using System;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.TinyCLR.Sockets.Channel
{
    /// <summary>
    /// Represents an authenticated ssl stream builder for server side applications.
    /// </summary>
    public class SslStreamBuilder
    {
        private TimeSpan _handshakeTimeout = TimeSpan.FromMilliseconds(10000);

        /// <summary>
        /// Initializes a new instance of the <see cref="SslStreamBuilder"/> class.
        /// </summary>
        public SslStreamBuilder(X509Certificate certificate, SslProtocols allowedProtocols)
        {
            Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            Protocols = allowedProtocols;
        }

        /// <summary>
        /// Builds a new <see cref="SslStream"/> from a connected socket.
        /// </summary>
        /// <param name="socket">The connected socket to create a stream with.</param>
        public NetworkStream Build(Socket socket)
        {
            NetworkStream stream;

            try
            {
                SslStream sslStream = new SslStream(socket);
                sslStream.AuthenticateAsServer(Certificate, Protocols);
                sslStream.ReadTimeout = (int)HandshakeTimeout.TotalMilliseconds;

                stream = sslStream;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"Handshake was not completed within the given interval.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to authenticate {socket.RemoteEndPoint}.", ex);
            }

            return stream;
        }

        /// <summary>
        /// Amount of time to wait for the TLS handshake to complete.
        /// </summary>
        public TimeSpan HandshakeTimeout
        {
            get => _handshakeTimeout;
            set
            {
                if (value <= TimeSpan.Zero && value <= TimeSpan.FromMilliseconds(int.MaxValue))
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _handshakeTimeout = value;
            }
        }

        /// <summary>
        /// The X.509 certificate.
        /// </summary>
        public X509Certificate Certificate { get; private set; }

        /// <summary>
        /// Allowed versions of ssl protocols.
        /// </summary>
        public SslProtocols Protocols { get; private set; }
    }
}
