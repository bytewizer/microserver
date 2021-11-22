namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Pass()
        {
            _context.Request.IsAuthenticated = true;
            _context.Response.Write(230, "User logged in.");

            // TODO: Auth Model

            //var lines = new string[] { "User cannot log in.", "Logon failure: Unknown user name or bad password." };
            //_context.Channel.Write(530, lines, "End");
        }
    }
}