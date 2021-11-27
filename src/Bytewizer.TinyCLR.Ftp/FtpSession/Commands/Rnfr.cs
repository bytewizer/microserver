using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Rnfr()
        {
            var feature = (SessionFeature)_context.Features.Get(typeof(ISessionFeature));
            feature.FromPath = _context.Request.Command.Argument;

            _context.Response.Write(350, $"Waiting for RNTO.");
        }
    }
}