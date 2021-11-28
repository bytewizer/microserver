namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>TYPE</c> command.
        /// </summary>
        private void Type()
        {     
            switch (_context.Request.Argument)
            {
                case "I":
                    _context.Request.TransferType = TransferType.Image;
                    _context.Response.Write(200, "Binary transfer mode active.");
                    break;

                case "A":
                    _context.Request.TransferType = TransferType.Ascii;
                    _context.Response.Write(200, "ASCII transfer mode active.");
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