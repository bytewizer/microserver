namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Dele()
        {
            var file = _context.Request.Command.Argument;

            try
            {
                _fileProvider.Delete(file);
                _context.Response.Write(250, $"File deleted successfull.");

            }
            catch
            {
                _context.Response.Write(500, $"File not found.");
            }
        }
    }
}