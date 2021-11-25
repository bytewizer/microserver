namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Dele()
        {
            var localPath = $"{_fileProvider.GetLocalDirectory()}\\{_context.Request.Command.Argument}";
            var remotePath = $"{_fileProvider.GetWorkingDirectory()}/{_context.Request.Command.Argument}";

            try
            {
                _fileProvider.Delete(localPath);
                _context.Response.Write(250, $"File {remotePath} deleted successfull.");

            }
            catch
            {
                _context.Response.Write(500, $"File {remotePath} not found.");
            }
        }
    }
}