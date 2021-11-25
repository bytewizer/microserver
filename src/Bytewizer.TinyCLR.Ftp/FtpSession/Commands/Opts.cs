namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Opts()
        {  
            switch (_context.Request.Command.Argument.ToUpper())
            {
                case "UTF8 ON":
                    _context.Response.Write(200, "UTF-8 is on.");
                    break;

                case "UTF8 OFF":
                    CommandNotImplemented();
                    break;

                default:
                    ParameterNotImplemented();
                    return;
            }
        }
    }
}