namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>MKD</c> command.
        /// </summary>
        private void Mkd()
        {
            var path = _context.Request.Command.Argument;

            try
            {
                _fileProvider.CreateDirectory(path);
                _context.Response.Write(257, $"Directory created successfull.");

            }
            catch
            {
                _context.Response.Write(500, $"CDUP command failed.");
            }
        }
    }
}