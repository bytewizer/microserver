namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Syst()
        {
            _context.Response.Write(215, $"UNIX simulated by TinyCLR OS.");
        }
    }
}