using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Rnfr()
        {
            var feature = new SessionFeature()
            {
                FromPath = _context.Request.Command.Argument
            };

            _context.Features.Set(typeof(ISessionFeature), feature);
            _context.Response.Write(350, $"Waiting for RNTO.");
        }
    }
}