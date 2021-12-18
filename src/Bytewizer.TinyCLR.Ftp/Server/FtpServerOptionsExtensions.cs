using System;
using System.Security.Cryptography.X509Certificates;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Extension methods for <see cref="FtpServerOptions"/> that configures <see cref="SocketListener"/> for a given endpoint.
    /// </summary>
    public static class FtpServerOptionsExtensions
    {
        /// <summary>
        /// Configure <see cref="SocketListener"/> to use a X509 certificate.
        /// </summary>
        /// <param name="serverOptions">The <see cref="SocketListenerOptions"/> to configure.</param>
        /// <param name="serverCertificate">The X.509 certificate.</param>
        public static FtpServerOptions UseImplicit(this FtpServerOptions serverOptions, X509Certificate serverCertificate)
        {
            if (serverOptions == null)
            {
                throw new ArgumentNullException(nameof(serverOptions));
            }

            if (serverCertificate == null)
            {
                throw new ArgumentNullException(nameof(serverCertificate));
            }

            if (serverOptions.SecurityMethod != SecurityMethod.None)
            {
                throw new InvalidOperationException("Only use one");
            }

            serverOptions.Listen(990, listener =>
            {
                listener.UseTls(serverCertificate);
            });

            serverOptions.Certificate = serverCertificate;
            serverOptions.SecurityMethod = SecurityMethod.Implicit;

            return serverOptions;
        }

        /// <summary>
        /// Configure <see cref="SocketListener"/> to use a X509 certificate.
        /// </summary>
        /// <param name="serverOptions">The <see cref="SocketListenerOptions"/> to configure.</param>
        /// <param name="serverCertificate">The X.509 certificate.</param>
        public static FtpServerOptions UseExplicit(this FtpServerOptions serverOptions, X509Certificate serverCertificate)
        {
            if (serverOptions == null)
            {
                throw new ArgumentNullException(nameof(serverOptions));
            }

            if (serverCertificate == null)
            {
                throw new ArgumentNullException(nameof(serverCertificate));
            }

            if(serverOptions.SecurityMethod != SecurityMethod.None)
            {
                throw new InvalidOperationException("Only use one");
            }

            serverOptions.Listen(21, listener =>
            {
                listener.UseCert(serverCertificate);
            });

            serverOptions.Certificate = serverCertificate;
            serverOptions.SecurityMethod = SecurityMethod.Explicit;

            return serverOptions;
        }
    }
}