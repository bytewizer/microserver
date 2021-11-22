using System.IO;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Cdup()
        {
            try
            {
                var path = _fileProvider.GetLocalDirectory();
                var directoryInfo = new DirectoryInfo(path);

                _fileProvider.SetWorkingDirectory(directoryInfo.Parent.FullName);
                _context.Response.Write(250, $"Working directory changed to {_fileProvider.GetWorkingDirectory()}.");
            }
            catch
            {
                _context.Response.Write(550, $"CDUP command failed.");
            }
        }
    }
}