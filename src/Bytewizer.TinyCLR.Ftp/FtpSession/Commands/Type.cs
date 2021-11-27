namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Type()
        {     
            switch (_context.Request.Argument)
            {
                case "I":
                    _context.Request.TransferType = TransferType.Image;
                    _context.Response.Write(215, $"TYPE set to IMAGE mode.");
                    break;

                case "A":
                    _context.Request.TransferType = TransferType.Ascii;
                    _context.Response.Write(215, $"TYPE set to ASCII mode.");
                    break;

                case "E":
                case "L":
                    ParameterNotImplemented();
                    break;

                default:
                    ParameterNotRecognized();
                    break;
            }
        }
    }
}