namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>STRU</c> command.
        /// </summary>
        private void Stru()
        {            
            switch (_context.Request.Argument)
            {
                case "F":
                    _context.Request.StructureType = StructureType.File;
                    _context.Response.Write(200, $"Structure set to File.");
                    break;

                case "R":
                case "P":
                    ParameterNotImplemented();
                    break;

                default:
                    ParameterNotRecognized();
                    break;
            }
        }
    }
}