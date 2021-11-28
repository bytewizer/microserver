namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>MODE</c> command.
        /// </summary>
        private void Mode()
        {            
            switch (_context.Request.Argument)
            {
                case "S":
                    _context.Request.TransferMode = TransferMode.Stream;
                    _context.Response.Write(200, $"MODE set to stream mode.");
                    break;

                case "B":
                case "C":
                case "D":
                    ParameterNotImplemented();
                    break;

                default:
                    ParameterNotRecognized();
                    break;
            }
        }
    }
}