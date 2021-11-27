namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Epsv()
        {
            var port = _listener.ActivePort;

            if (port != -1)
            {
                _context.Request.DataMode = DataMode.ExtendedPassive;
                _context.Channel.Write(229, $"Entering extended passive mode (|||{port}|)");
            }
            else
            {
                _context.Request.DataMode = DataMode.None;
                _context.Channel.Write(500, "EPSV command failed.");
            }
        }
    }
}