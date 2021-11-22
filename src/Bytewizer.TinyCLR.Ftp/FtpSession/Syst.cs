namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Syst()
        {
            string format = string.Empty;

            switch (_context.Request.ListFormat)
            {
                case ListFormat.MsDos:
                    _context.Request.ListFormat = ListFormat.MsDos;
                    format = "MS DOS";
                    break;

                case ListFormat.Unix:
                    _context.Request.ListFormat = ListFormat.Unix;
                    format = "UNIX";
                    break;
            }

            _context.Response.Write(215, $"{format} simulated by TinyCLR OS.");
        }
    }
}