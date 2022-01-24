using System.Net;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>PORT</c> commands.
        /// </summary>
        private void Port()
        {
            try
            {
                var addr = _context.Request.Command.Argument.Split(new char[] { ',' });
                var ip = $"{addr[0]}.{addr[1]}.{addr[2]}.{addr[3]}";
                var port = (int.Parse(addr[4]) * 0x100) + int.Parse(addr[5]);

                _endpoint = new IPEndPoint(IPAddress.Parse(ip), port);

                _context.Request.DataMode = DataMode.Active;
                _context.Response.Write(200, "PORT command successful.");
            }
            catch
            {
                _context.Request.DataMode = DataMode.None;
                _context.Response.Write(500, "PORT command failed.");
            }
        }
    }
}