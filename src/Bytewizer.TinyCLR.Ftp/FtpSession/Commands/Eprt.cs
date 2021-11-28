using System.Net;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>EPRT</c> command.
        /// </summary>
        private void Eprt()
        { 
            try
            {
                var addr = _context.Request.Command.Argument.Split(new char[] { ',' });
                var ip = $"{addr[0]}.{addr[1]}.{addr[2]}.{addr[3]}";
                var port = (int.Parse(addr[4]) * 0x100) + int.Parse(addr[5]);

                _endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                if (_endpoint == null)
                {
                    _context.Response.Write(501, "Syntax error in parameters or arguments.");
                }

                _context.Request.DataMode = DataMode.ExtendedActive;
                _context.Response.Write(200, "EPRT command successful.");
            }
            catch
            {
                _context.Request.DataMode = DataMode.None;
                _context.Response.Write(500, "EPRT command failed.");
            }
        }
    }
}