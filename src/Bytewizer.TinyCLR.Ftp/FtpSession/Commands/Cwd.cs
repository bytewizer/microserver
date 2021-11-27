namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Cwd()
        {            
            if (_fileProvider.SetWorkingDirectory(_context.Request.Command.Argument))
            {
                _context.Response.Write(250, $"Working directory changed to {_fileProvider.GetWorkingDirectory()}.");
            }
            else
            {
                _context.Response.Write(550, "No such directory.");
            }
        }
    }
}