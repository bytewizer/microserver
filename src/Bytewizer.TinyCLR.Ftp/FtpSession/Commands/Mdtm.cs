using System;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Mdtm()
        {
            var path = _context.Request.Command.Argument;

            try
            {
                var time = _fileProvider.GetLastWriteTime(path);
                if (time != DateTime.MinValue)
                {
                    _context.Response.Write(213, time.ToUniversalTime().ToString("yyyyMMddHHmmss.fff"));
                }
                else
                {
                    _context.Response.Write(500, $"File not found.");
                }
            }
            catch
            {
                _context.Response.Write(550, $"MDTM command failed.");
            }
        }
    }
}