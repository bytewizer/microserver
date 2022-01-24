using Bytewizer.TinyCLR.Sockets.Channel;
using System.Security.Authentication;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// The <c>AUTH</c> command handler.
        /// </summary>
        private void Auth()
        {
            if (_context.Request.SecurityType != SecurityType.None)
            {
                _context.Response.Write(534, "Secure connection was already negotiated.");
                return;
            }

            switch (_context.Request.Argument)
            {
                case "TLS-C":
                case "TLS":
                    _context.Request.SecurityType = SecurityType.Tls;

                    //write to channel
                    _context.Channel.Write(234, "AUTH command OK. Initializing SSL connection.");

                    var channel = new SocketChannel();
                    channel.Assign(_context.Channel.Client, _ftpOptions.Certificate, SslProtocols.Tls12);
                    _context.Channel = channel;

                    //_context.Response.Write(334, "");

                    break;

                case "TLS-P":
                case "SSL":
                    ParameterNotImplemented();
                    break;

                default:
                    ParameterNotRecognized();
                    break;
            }
        }
    }
}