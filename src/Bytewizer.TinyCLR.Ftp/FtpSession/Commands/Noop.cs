namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Noop()
        {
            _context.Response.Write(200, "NOOP command successfull.");
        }
    }
}