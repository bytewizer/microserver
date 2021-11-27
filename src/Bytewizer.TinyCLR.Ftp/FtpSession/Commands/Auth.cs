namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
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
                    _context.Response.Write(215, $"Transport security set to TLS mode.");
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