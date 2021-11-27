namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Pasv()
        {
            try
            {
                var localEndpoint = _context.Connection.LocalIpAddress;
                var ipBytes = localEndpoint.GetAddressBytes();

                var port = _listener.ActivePort;

                var passiveEndpoint =
                    string.Format(
                        "{0},{1},{2},{3},{4},{5}",
                        ipBytes[0],
                        ipBytes[1],
                        ipBytes[2],
                        ipBytes[3],
                        (port & 0xFF00) >> 8,
                        (port & 0x00FF));

                _context.Request.DataMode = DataMode.Passive;
                _context.Response.Write(227, $"Entering passive mode ({passiveEndpoint}).");
            }
            catch
            {
                _context.Request.DataMode = DataMode.None;
                _context.Response.Write(500, "PASV command failed.");
            }
        }
    }
}