namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Quit()
        {
            _context.Response.Write(221, "Goodbye.");
            _context.Channel.Client.Close();
        }
    }
}