namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Size()
        {
            var argument = _context.Request.Command.Argument;
            var remotePath = $"{_fileProvider.GetWorkingDirectory()}/{_context.Request.Command.Argument}";

            try
            {
                var size = _fileProvider.GetFileSize(argument);
                if (string.IsNullOrEmpty(size))
                {
                    _context.Response.Write(213, size);
                }
                else
                {
                    _context.Response.Write(500, $"File {remotePath} not found.");
                }
            }
            catch
            {
                _context.Response.Write(550, $"SIZE command failed.");
            }
        }
    }
}