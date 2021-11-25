namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Mdmt()
        {
            var argument = _context.Request.Command.Argument;
            var remotePath = $"{_fileProvider.GetWorkingDirectory()}/{_context.Request.Command.Argument}";

            try
            {
                var time = _fileProvider.GetLastWriteTime(argument);
                if (!string.IsNullOrEmpty(time))
                {
                    _context.Response.Write(213, time);
                }
                else
                {
                    _context.Response.Write(500, $"File {remotePath} not found.");
                }
            }
            catch
            {
                _context.Response.Write(550, $"MDMT command failed.");
            }
        }
    }
}