using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketServer"/> for creating network servers.
    /// </summary>
    public class FtpServerOptions : ServerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        public FtpServerOptions()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            BannerMessage = $"TinyCLR FTP Server Ready [{version}]";
        }
        /// <summary>
        /// Specifies the FTPS security method.
        /// </summary>
        internal SecurityMethod SecurityMethod { get; set; } = SecurityMethod.None;

        /// <summary>
        /// Specifies the X.509 certificate for the FTPS security method.
        /// </summary>
        public X509Certificate Certificate { get; internal set; }

        /// <summary>
        /// Gets or sets the root directory path.
        /// </summary>
        public string RootPath { get; set; } = @"A:\";

        /// <summary>
        /// Specifies the server banner message.
        /// </summary>
        public string BannerMessage { get; set; }

        /// <summary>
        /// Gets the public address of the server.
        /// </summary>
        /// <remarks>
        /// If server is hosted behind NAT, then passive mode may not work correctly, 
        /// as it will always use local IP address. This property defines a public hostname
        /// or an external IP address of the server.
        /// </remarks>
        public IPAddress PublicAddress { get; private set; }
    }
}