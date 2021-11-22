using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Rnto()
        {
            var fromPath = ((SessionFeature)_context.Features.Get(typeof(ISessionFeature))).FromPath;
            if (string.IsNullOrEmpty(fromPath))
            {
                _context.Response.Write(503, $"Wrong sequence renaming aborted");
                return;
            }

            var toPath = _context.Request.Command.Argument;
            try
            {
                _fileProvider.Rename(fromPath, toPath);
                _context.Response.Write(250, $"Rename succeeded.");

            }
            catch
            {
                _context.Response.Write(500, $"Rename failed.");
            }
        }
    }
}