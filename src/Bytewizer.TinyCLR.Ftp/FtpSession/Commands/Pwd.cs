using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Pwd()
        {
            var path = _fileProvider.GetWorkingDirectory();
            _context.Response.Write(257, $"\"{path}\" is current directory.");
        }
    }
}