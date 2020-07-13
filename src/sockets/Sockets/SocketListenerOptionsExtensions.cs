using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.Sockets
{
    public static class SocketListenerOptionsExtensions
    {
        public static SocketListenerOptions UseHttps(this SocketListenerOptions listenOptions, X509Certificate serverCertificate)
        {
            if (serverCertificate == null)
                throw new ArgumentNullException(nameof(serverCertificate));
            
            listenOptions.Certificate = serverCertificate;
            listenOptions.IsTls = true;
            listenOptions.KeepAlive = true;
            
            return listenOptions;
        }

        public static SocketListenerOptions UseTcp(this SocketListenerOptions listenOptions)
        {
            listenOptions.SocketType = SocketType.Stream;
            listenOptions.ProtocolType = ProtocolType.Tcp;
            listenOptions.ReuseAddress = true;
            return listenOptions;
        }

        public static SocketListenerOptions UseUdp(this SocketListenerOptions listenOptions)
        {
            listenOptions.SocketType = SocketType.Dgram;
            listenOptions.ProtocolType = ProtocolType.Udp;
            return listenOptions;
        }
    }
}