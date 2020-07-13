using System;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.Sockets
{

    class SslStreamBuilder
    {
        private TimeSpan _handshakeTimeout = TimeSpan.FromMilliseconds(10000);

        public SslStreamBuilder(X509Certificate certificate, SslProtocols allowedProtocols)
        {
            Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            Protocols = allowedProtocols;
        }

        public SslStream Build(Socket socket)
        {
            SslStream stream;

            try
            {
                stream = new SslStream(socket);
                stream.AuthenticateAsServer(Certificate, Protocols);
                stream.ReadTimeout = (int)HandshakeTimeout.TotalMilliseconds;
            }
            catch (InvalidOperationException)
            {
               throw new InvalidOperationException($"Handshake was not completed within the given interval.");
            }

            catch (IOException ex)
            {
                throw new InvalidOperationException($"Failed to authenticate {socket.RemoteEndPoint}.", ex);
            }

            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to authenticate {socket.RemoteEndPoint}.", ex);
            }

            return stream;
        }

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

        public X509Certificate Certificate { get; private set; }

        public SslProtocols Protocols { get; set; }
    }
}
