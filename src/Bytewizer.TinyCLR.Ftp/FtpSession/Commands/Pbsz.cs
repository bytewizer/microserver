using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Pbsz()
        {
            if (_context.Request.SecurityType == SecurityType.None)
            {
                BadSequenceOfCommands();
                return;
            }

            try
            {
                var feature = new SessionFeature()
                {
                    TlsBlockSize = int.Parse(_context.Request.Command.Argument)
                };

                _context.Features.Set(typeof(ISessionFeature), feature);
                _context.Response.Write(350, "PBSZ command successfull.");
            }
            catch
            {
                _context.Response.Write(500, "PBSZ command failed.");
            }
        }
    }
}