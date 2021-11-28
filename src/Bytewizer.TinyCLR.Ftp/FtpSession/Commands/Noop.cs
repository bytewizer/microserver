namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>NOOP</c> command.
        /// </summary>
        private void Noop()
        {
            _context.Response.Write(200, "NOOP command successfull.");
        }
    }
}