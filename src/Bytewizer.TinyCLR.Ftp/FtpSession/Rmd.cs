namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Rmd()
        {
            var localPath = $"{_fileProvider.GetLocalDirectory()}\\{_context.Request.Command.Argument}";
            var remotePath = $"{_fileProvider.GetWorkingDirectory()}/{_context.Request.Command.Argument}";

            try
            {
                _fileProvider.DeleteDirectory(localPath);
                _context.Response.Write(250, $"Directory {remotePath} deleted successfull.");

            }
            catch
            {
                _context.Response.Write(500, $"Directory {remotePath} not found.");
            }
        }
    }
}