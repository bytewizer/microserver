namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Pwd()
        {
            var message = _fileProvider.GetWorkingDirectory();
            _context.Response.Write(257, $"{message} is current directory.");
        }
    }
}