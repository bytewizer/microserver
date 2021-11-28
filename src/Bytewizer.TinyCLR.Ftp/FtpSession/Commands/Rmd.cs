namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>RMD</c> command.
        /// </summary>
        private void Rmd()
        {
            var path = _context.Request.Command.Argument;

            try
            {
                _fileProvider.DeleteDirectory(path);
                _context.Response.Write(250, "Directory removed.");

            }
            catch
            {
                _context.Response.Write(550, "Not a valid directory.");
            }
        }
    }
}