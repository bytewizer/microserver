namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Epsv()
        {
            //_listener.Start();
            _context.Request.DataMode = DataMode.ExtendedPassive;
            _context.Channel.Write(229, $"Entering extended passive mode (|||{_listener.ActivePort}|)");
        }
    }
}