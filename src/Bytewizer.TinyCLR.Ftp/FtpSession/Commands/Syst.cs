namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>SYST</c> command.
        /// </summary>
        private void Syst()
        {
            _context.Response.Write(215, "UNIX simulated by TinyCLR OS.");
        }
    }
}