using System;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>MDTM</c> command.
        /// </summary>
        private void Mdtm()
        {
            var path = _context.Request.Command.Argument;

            try
            {
                if (_fileProvider.GetLastWriteTime(path, out DateTime dateTime))
                {
                    _context.Response.Write(213, dateTime.ToTimeString());
                }
                else
                {
                    _context.Response.Write(550, "File not found.");
                }
            }
            catch
            {
                _context.Response.Write(500, "MDTM command failed.");
            }
        }
    }
}