namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Eprt()
        { 
            try
            {
                //TODO
                _context.Request.DataMode = DataMode.ExtendedActive;
                _context.Response.Write(200, $"Data Connection Established");
            }
            catch
            {
                _context.Request.DataMode = DataMode.None;
                _context.Response.Write(500, "EPRT command failed.");
            }
        }
    }
}