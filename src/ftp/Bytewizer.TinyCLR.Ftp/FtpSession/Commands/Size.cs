namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>SIZE</c> command.
        /// </summary>
        private void Size()
        {
            var file = _context.Request.Command.Argument;

            try
            {
                if (_fileProvider.GetFileSize(file, out long size))
                {
                    _context.Response.Write(213, size.ToString());
                }
                else
                {
                    _context.Response.Write(550, "File not found.");
                }
            }
            catch
            {
                _context.Response.Write(500, "SIZE command failed.");
            }
        }
    }
}