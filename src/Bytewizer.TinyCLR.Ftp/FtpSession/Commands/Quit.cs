namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>QUIT</c> command.
        /// </summary>
        private void Quit()
        {
            _context.Response.Write(221, "Service closing control connection.");
            _context.Active = false;
        }
    }
}