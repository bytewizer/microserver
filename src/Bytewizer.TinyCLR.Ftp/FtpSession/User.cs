namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void User()
        {
            //_context.User = _context.Request.Command.Argument;
            //if (_context.User == "anonymous" && _ftpOptions.AllowAnonymous)
            //{
            _context.Response.Write(331, "Anonymous access allowed, send identity (e-mail name) as password.");
            //}
            //else
            //{
            //    _context.Response.Write(331, $"Password required for {_context.User}.");
            //}
        }
    }
}