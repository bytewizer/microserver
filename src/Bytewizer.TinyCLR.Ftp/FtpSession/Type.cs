namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Type()
        {
            string datatype;
            
            switch (_context.Request.Command.Argument)
            {
                case "A":
                    _context.Request.DataType = DataType.Ascii;
                    datatype = "ASCII";
                    break;

                case "I":
                    datatype = "IMAGE";
                    _context.Request.DataType = DataType.Image;
                    break;

                default:
                    ParameterNotImplemented();
                    return;
            }

            _context.Response.Write(215, $"TYPE set to {datatype}.");
        }
    }
}