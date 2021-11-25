using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void User()
        {
            var AllowAnonymous = true;

            var feature = (FtpAuthenticationFeature)_context.Features.Get(typeof(IFtpAuthenticationFeature));
            if (feature != null)
            {
                AllowAnonymous = feature.AllowAnonymous;
            }

            if (_context.Request.Argument == "ANONYMOUS" && AllowAnonymous)
            {
                _context.Response.Write(331, "Anonymous access allowed, send identity (e-mail name) as password.");
            }
            else
            {
                _context.Response.Write(331, $"Password required for {_context.Request.Command.Argument}.");
            }

            _context.Request.Name = _context.Request.Command.Argument;
        }
    }
}