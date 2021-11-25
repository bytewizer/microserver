namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Stru()
        {            
            switch (_context.Request.Argument)
            {
                case "F":
                    _context.Request.StructureType = StructureType.File;
                    _context.Response.Write(215, $"STRU set to file mode.");
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