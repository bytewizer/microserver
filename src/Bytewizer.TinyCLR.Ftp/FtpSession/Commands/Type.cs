namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Type()
        {     
            switch (_context.Request.Argument)
            {
                case "I":
                    _context.Response.Write(215, $"TYPE set to image mode.");
                    _context.Request.TransferType = TransferType.Image;
                    break;

                case "A":
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