namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Mkd()
        {
            var localPath = $"{_fileProvider.GetLocalDirectory()}\\{_context.Request.Command.Argument}";
            var remotePath = $"{_fileProvider.GetWorkingDirectory()}/{_context.Request.Command.Argument}";

            try
            {
                _fileProvider.CreateDirectory(localPath);
                _context.Response.Write(250, $"Directory {remotePath} created successfull.");

            }
            catch
            {
                _context.Response.Write(500, $"CDUP command failed.");
            }
        }
    }
}