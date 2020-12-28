using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Extension methods for <see cref="SocketListenerOptions"/> that configures <see cref="SocketListener"/> for a given endpoint.
    /// </summary>
    public static class SocketListenerOptionsExtensions
    {
        /// <summary>
        /// Configure <see cref="SocketListener"/> to use HTTPS.
        /// </summary>
        /// <param name="listenOptions">The <see cref="SocketListenerOptions"/> to configure.</param>
        /// <param name="serverCertificate">The X.509 certificate.</param>
        public static SocketListenerOptions UseHttps(this SocketListenerOptions listenOptions, X509Certificate serverCertificate)
        {
            if (listenOptions == null)
            {
                throw new ArgumentNullException(nameof(listenOptions));
            }

            if (serverCertificate == null)
            {
                throw new ArgumentNullException(nameof(serverCertificate));
            }

            listenOptions.Certificate = serverCertificate;
            listenOptions.IsTls = true;
            listenOptions.KeepAlive = true;
            
            return listenOptions;
        }

        /// <summary>
        /// Configure <see cref="SocketListener"/> to use TCP.
        /// </summary>
        /// <param name="listenOptions">The <see cref="SocketListenerOptions"/> to configure.</param>
        public static SocketListenerOptions UseTcp(this SocketListenerOptions listenOptions)
        {
            if (listenOptions == null)
            {
                throw new ArgumentNullException(nameof(listenOptions));
            }

            listenOptions.SocketType = SocketType.Stream;
            listenOptions.ProtocolType = ProtocolType.Tcp;
            listenOptions.ReuseAddress = true;
            
            return listenOptions;
        }

        /// <summary>
        /// Configure <see cref="SocketListener"/> to use UDP.
        /// </summary>
        /// <param name="listenOptions">The <see cref="SocketListenerOptions"/> to configure.</param>
        public static SocketListenerOptions UseUdp(this SocketListenerOptions listenOptions)
        {
            if (listenOptions == null)
            {
                throw new ArgumentNullException(nameof(listenOptions));
            }

            listenOptions.SocketType = SocketType.Dgram;
            listenOptions.ProtocolType = ProtocolType.Udp;
            
            return listenOptions;
        }
    }
}