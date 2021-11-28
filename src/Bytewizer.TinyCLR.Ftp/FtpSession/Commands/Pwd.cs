using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>PWD</c> command.
        /// </summary>
        private void Pwd()
        {
            try
            {
                var path = _fileProvider.GetWorkingDirectory();
                _context.Response.Write(257, $"\"{path}\"");
            }
            catch
            {
                _context.Response.Write(500, "PWD command failed.");
            }
        }
    }
}