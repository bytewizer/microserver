using System.IO;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>CDUP</c> command.
        /// </summary>
        private void Cdup()
        {
            try
            {
                var path = _fileProvider.GetLocalDirectory();
                var directoryInfo = new DirectoryInfo(path);

                _fileProvider.SetWorkingDirectory(directoryInfo.Parent.FullName);
                _context.Response.Write(200, $"Working directory changed to {_fileProvider.GetWorkingDirectory()}.");
            }
            catch
            {
                _context.Response.Write(550, "Not a valid directory.");
            }
        }
    }
}