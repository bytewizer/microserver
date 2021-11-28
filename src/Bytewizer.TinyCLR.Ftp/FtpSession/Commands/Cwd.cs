namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>CWD</c> command.
        /// </summary>
        private void Cwd()
        {
            var path = _context.Request.Command.Argument;

            if (_fileProvider.SetWorkingDirectory(path))
            {
                _context.Response.Write(250, $"Successful ({_fileProvider.GetWorkingDirectory()})");
            }
            else
            {
                _context.Response.Write(550, "Not a valid directory.");
            }
        }
    }
}