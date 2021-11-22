using System.Net;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Pasv()
        {
            var localEP = _context.Connection.LocalIpAddress;
            var ipBytes = localEP.GetAddressBytes();

            //_listener.Start();

            var port = ((IPEndPoint)_listenSocket.LocalEndPoint).Port;

            var passiveEP =
                string.Format(
                    "{0},{1},{2},{3},{4},{5}",
                    ipBytes[0],
                    ipBytes[1],
                    ipBytes[2],
                    ipBytes[3],
                    (port & 0xFF00) >> 8,
                    (port & 0x00FF));

            _context.Request.DataMode = DataMode.Passive;
            _context.Channel.Write(227, $"Entering passive mode ({passiveEP})");
        }

    }
}