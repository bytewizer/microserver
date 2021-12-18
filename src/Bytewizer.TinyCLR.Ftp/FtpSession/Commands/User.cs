using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>USER</c> command.
        /// </summary>
        private void User()
        {
            var AllowAnonymous = true;

            var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));
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